using CardService.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace CardService.Utils.Validators.Attributes
{
    public class CardDateValidatorAttribute:ValidationAttribute
    {
         protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
                if (value is CardDateExpired date)
                {
                    if ((date.Month < 1) || (date.Month > 12) ||
                                        (date.Month < DateTime.Now.Year))

                        return new ValidationResult("error");
                }
            return ValidationResult.Success;
        }
    }
}
