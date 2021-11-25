using CardService.Models.Request;
using CardService.Services.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace CardService.Filters
{
    /// <summary>
    /// Filter cheks card params for valid
    /// </summary>
    public class CardDataValidatorFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var addCardRequestModel = context.ActionArguments["card"] as ModelToAddCardDto;

            if (!CardParamsValidator.IsCardExpireDateValidateTrue(addCardRequestModel.Month,
                                                                 addCardRequestModel.Year))
                context.ModelState.AddModelError("date", "date expire error validation");

            if (!CardParamsValidator.IsCardPanValidateTrue(addCardRequestModel.Pan))
                context.ModelState.AddModelError("pan", "PAN error validation");
        }

    }
}
