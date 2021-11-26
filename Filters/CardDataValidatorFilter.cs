using CardService.Models;
using CardService.Models.Request;
using CardService.Services.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;

namespace CardService.Filters
{
    /// <summary>
    /// Filter cheks card params for valid
    /// </summary>
    public class CardDataValidatorFilter : Attribute, IActionFilter
    {
        ApiResponseModel _model;
        ErrorMessage _errorMessage;
        
        public CardDataValidatorFilter(ApiResponseModel model)
        {
            _model = model;
            _model.IsOkStatus = false;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
          
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var addCardRequestModel = context.ActionArguments["card"] as ModelToAddCardDto;

            if (!CardParamsValidator.IsCardExpireDateValidateTrue(addCardRequestModel.Month,
                                                                 addCardRequestModel.Year))
            {
                _model.ErrorMessage.Message = "date expire error validation";
                var errorMessage = JsonConvert.SerializeObject(_model);
                context.HttpContext.Response.WriteAsync(errorMessage);
            }

            if (!CardParamsValidator.IsCardPanValidateTrue(addCardRequestModel.Pan))
            { 
                _model.ErrorMessage.Message = "PAN error validation";
                var errorMessage = JsonConvert.SerializeObject(_model);
                context.HttpContext.Response.WriteAsync(errorMessage);
            }
        }
    }
}
