using CardService.Domain;
using CardService.Models.Request;
using System;
using System.ComponentModel.DataAnnotations;

namespace CardService.Utils.Validators.Attributes
{
    public class CardDateValidatorAttribute:ValidationAttribute
    {
         protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
                if (value is CardDate date)
                {
                    if ((date.Month < 1) || (date.Month > 12) ||
                                        (date.Year < DateTime.Now.Year))

                        return new ValidationResult("Card Date expired error");
                }
            return ValidationResult.Success;
        }
    }
}
