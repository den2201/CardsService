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
        //  services.AddHostedService<DbCardsValidationService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ICardRepository cardRepository)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
