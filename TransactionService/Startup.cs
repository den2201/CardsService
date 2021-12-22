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
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TransactionService.Apis;
using TransactionService.Datalayer;
using TransactionService.Services;
using TransactionService.Services.Repository;
using Microsoft.Extensions.Http;
using Polly.Extensions.Http;
using System.Net;
using Polly.Timeout;
using Polly;

namespace TransactionService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services, ILogger<Startup> logger)
        {

            services.AddControllers();
            services.AddLogging();
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(Configuration["ConnectionStrings:PostgresConnString"]));
            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.Host(new Uri("rabbitmq://localhost:5672"), h =>
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TransactionService", Version = "v1" });
            });
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddOptions();
            services.Configure<CardServiceSettings>(Configuration.GetSection("CardServiceApi"));
            services.AddHttpClient<ICardApiClient, CardServiceApiClient>(t =>
            {
                t.BaseAddress = new Uri("http://localhost:6000");
            }).AddHttpMessageHandler(provider =>
            {
                return new LoggingHAndler(provider.GetRequiredService<IHttpContextAccessor>());
            }).SetHandlerLifetime(TimeSpan.FromMinutes(5)).AddPolicyHandler(p =>
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
                                                            logger.LogError($"CircuitBreaker onBreak for {timeSpan.TotalMilliseconds} ms"),
                                                  context => service.GetService<ILogger>().LogError("CircuitBreaker onReset")));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TransactionService v1"));
            }
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
