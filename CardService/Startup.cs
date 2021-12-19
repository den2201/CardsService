using CardService.Services;
using CardService.Services.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CardService.Domain;
using CardService.Services.Logging;
using CardService.AppConfiguration;
using CardService.Filters;
using CardService.BackgroundTasks;
using CardService.Models;
using Microsoft.EntityFrameworkCore;
using CardService.Models.Request;
using SharedEntities.Models;
using MassTransit;
using CardService.Consumers;

namespace CardService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.AddMvc();
            services.Configure<AppSettings>(appSettingsSection);
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(Configuration["ConnectionStrings:PostgresConnString"]));
            services.AddTransient<ICardRepository, DbRepository>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CardsApiServise", Version = "v1.0" });
                var filePath = Path.Combine(System.AppContext.BaseDirectory, $"{nameof(CardService)}.xml");
                c.IncludeXmlComments(filePath);
            });
            services.AddTransient<ApiResponseModel<Card>>();
            services.AddControllers(options =>
            { 
                options.Filters.Add<LoggingFilter>(); 
            });
            services.AddMassTransit(x =>
            {
                x.AddConsumer<TransactionConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri("rabbitmq://localhost:5672"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ReceiveEndpoint("card-queue", ep =>
                    {
                        ep.ConfigureConsumer<TransactionConsumer>(provider);
                    });
                }));
            });
            services.AddMassTransitHostedService();


            //  services.AddHostedService<DbCardsValidationService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ICardRepository cardRepository)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, func) =>
            {
                var logger = context.RequestServices.GetService<ILogger<Startup>>();
                var requestId = context.Request.Headers["Request_ID"];
                if (string.IsNullOrEmpty(requestId))
                {
                    context.Request.Headers["Request_ID"] = Guid.NewGuid().ToString();
                }
                context.Items["REquest_ID"] = requestId;
                using (logger.BeginScope($"{requestId}"))
                {
                    await func();
                }
            });

            app.UseCors("AnyOrigin");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseMiddleware<RequestLogService>();
           
            app.UseRouting();
            
            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllers();
            });
           
            ///Terminating middleware
            app.Run(async (context) =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Something went wrong");
            });
        }
    }
}
