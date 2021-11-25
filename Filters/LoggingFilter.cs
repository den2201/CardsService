using CardService.Domain;
using CardService.Models;
using CardService.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CardService.Filters
{
    public class LoggingFilter : IResultFilter, IActionFilter
    {
        private readonly ILogger _logger;
        private JsonSerializerSettings settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        public LoggingFilter(ILogger<LoggingFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;

            if (context.HttpContext.Request.Method == "GET") 
            {
                _logger.LogInformation($"Request: Path:{request.Path.Value} to Action: {context.ActionDescriptor.DisplayName}");
                if (request.Query != null)
                {
                    foreach (var item in request.Query)
                    {
                        _logger.LogInformation($"paramseters: {nameof(item.Key)}: {item.Key} - {nameof(item.Value)}: {item.Value}");
                    }
                }
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.Result is ObjectResult okResult)
            {
                try
                {
                    var responseDataModel = okResult.Value as ApiResponseModel;
                    if (responseDataModel != null)
                    {

                        if ((context.HttpContext.Request.Method == "GET")
                           && context.ActionDescriptor.DisplayName.Contains("GetCardByUserId"))
                        {
                            if ((responseDataModel.Data is IList<Card> cards))
                            {
                                foreach (var card in cards)
                                {
                                    var card1 = new InvisibleParamsCardPtototypeCreater().Create(card);
                                    _logger.LogInformation($" {context.ActionDescriptor.DisplayName}:  card: " +
                                    $"{nameof(card.CardName)} : {card1.CardName} |" +
                                    $"{nameof(card.Pan)} : {card1.Pan} |" +
                                    $"{nameof(card.CVC)} : {card1.CVC} |" +
                                    $"{nameof(card.UserId)} : {card1.UserId}");

                                }
                            }
                        }

                        else if ((context.HttpContext.Request.Method == "POST")
                          && context.ActionDescriptor.DisplayName.Contains("UpdateCardName"))
                        {
                            if ((responseDataModel.IsOkStatus) && (responseDataModel.Data is Card card))
                            {
                                _logger.LogInformation($"Card Id: {card.Id} name is changed {card.CardName}");
                            }
                            else
                            {
                                _logger.LogError($"{DateTime.UtcNow}: {responseDataModel.ErrorMessage.Message} ");
                            }

                        }
                    }
                }

                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
        }

    }
    
}
