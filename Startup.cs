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
            services.Configure<AppSettings>(appSettingsSection);
            services.AddSingleton<ICardRepository, MemoryRepository>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ICardRepository cardRepository)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            ///
            /// logging middleware
            ///
            app.UseMiddleware<RequestLogService>();
           
            app.Map("/addcard", AddCard);
            app.Map("/getcard", GetCard);
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

            ///
            /// card adding middleware method
            ///
            void AddCard(IApplicationBuilder app)
            {
                var bodyString = "";
                app.Run(async (context) =>
                {
                    var bodyBytes = context.Request.Body;
                    Card card;
                    using (StreamReader reader = new StreamReader(bodyBytes))
                    {
                        bodyString = await reader.ReadToEndAsync();
                    }
                    try
                    {
                        card = JsonConvert.DeserializeObject<Card>(bodyString);
                         cardRepository.AddCard(card);
                    }
                    catch (Exception ex)
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsync(ex.Message);
                    }
                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsync("Card Added");
                });
            }
            ///
            /// cards getting middleware method
            ///
             void GetCard(IApplicationBuilder app)
            {
                app.Run(async (context) =>
                {
                    try
                    {
                        var value = context.Request.Path.Value;
                        Guid id = new();
                        string json = string.Empty;
                        var userIdParams = context.Request.Query.Where(x => x.Key == "userid").FirstOrDefault();
                        id = Guid.Parse(userIdParams.Value);
                        var cards = cardRepository.GetCardsByUserId(id);
                        if(cards.Count() > 0)
                          json = JsonConvert.SerializeObject(cards);

                        if (!string.IsNullOrEmpty(json))
                        {
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = StatusCodes.Status200OK;
                            await context.Response.WriteAsync(json);
                        }
                        else
                        {
                            context.Response.StatusCode = StatusCodes.Status404NotFound;
                            await context.Response.WriteAsync("Not Found");
                        }
                    }
                    catch(Exception ex)
                    {
                        context.Response.ContentType = "text/html";
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsync(ex.Message);
                    }
                });

            }
        }
    }
}
