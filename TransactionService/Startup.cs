using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SharedEntities.Models;
using System;
using Microsoft.AspNetCore.Authentication;
using TransactionService.Apis;
using TransactionService.Datalayer;
using TransactionService.Services;
using TransactionService.Services.Repository;
using Microsoft.Extensions.Http;
using Polly.Extensions.Http;
using System.Net;
using Polly.Timeout;
using Polly;
using System.IO;

namespace TransactionService
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

            services.AddControllers();
            services.AddLogging();
            services.AddAuthentication();
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(Configuration["ConnectionStrings:PostgresConnString"]));
            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.Host(new Uri("rabbitmq://rabbitmq:5672"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    config.ReceiveEndpoint("card-queue", c =>
                    {
                        c.Handler<CardDto>(context =>
                        {
                            return Console.Out.WriteLineAsync(context.Message.CardName);
                        });

                    });
                }));
            });
            services.AddMassTransitHostedService();
              services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TransactionApiServise", Version = "v1.0" });
                var filePath = Path.Combine(System.AppContext.BaseDirectory, $"{nameof(TransactionService)}.xml");
                c.IncludeXmlComments(filePath);
            });
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddOptions();
            services.Configure<CardServiceSettings>(Configuration.GetSection("CardServiceApi"));
            
            services.AddHttpClient<ICardApiClient, CardServiceApiClient>(t =>
            {
                t.BaseAddress = new Uri("http://localhost:6000");
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5)).AddPolicyHandler(p =>
                HttpPolicyExtensions
           .HandleTransientHttpError()
           .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
           .OrResult(msg => msg.StatusCode == HttpStatusCode.InternalServerError)
           .Or<TimeoutRejectedException>()
           .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) )
           )
            .AddPolicyHandler((service, request) =>
                                HttpPolicyExtensions.HandleTransientHttpError()
                                    .CircuitBreakerAsync(5,
                                                 TimeSpan.FromSeconds(30),
                                                 (result, timeSpan, context) =>
                                                            service.GetService<ILogger>().LogError($"CircuitBreaker onBreak for {timeSpan.TotalMilliseconds} ms"),
                                                  context => service.GetService<ILogger>().LogError("CircuitBreaker onReset")));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
            }
           app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
            context.Database.EnsureCreated();
            app.Use(async (context, func) =>
            {
                var logger = context.RequestServices.GetService<ILogger<Startup>>();
                var requestId = context.Request.Headers["Request_ID"];
                if(string.IsNullOrEmpty(requestId))
                {
                    context.Request.Headers["Request_ID"] = Guid.NewGuid().ToString();
                }
                context.Items["Request_ID"] = requestId;
                using (logger.BeginScope($"{requestId}"))
                {
                    await func();
                }
            });

            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
