using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CardService.Utils.Validators.Attributes
{
    public class PanValidatorAttribute:ValidationAttribute
    {
        const int panLength = 16;
        const int numberNine = 9;
        private static int[] cardPanNumbers = new int[panLength];
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string cardPanNumber = value.ToString();
            if ((string.IsNullOrEmpty(cardPanNumber) || (cardPanNumber.Length != 16)))
               return new ValidationResult("card number is empty or < 16 symbols");
            if (cardPanNumber.Where(x => char.IsWhiteSpace(x) || char.IsLetter(x)).Any())
               return new ValidationResult("incorrect symbols in card number");

            for (int i = 0; i < panLength; i++)
            {
                if (char.IsDigit(cardPanNumber[i]))
                    cardPanNumbers[i] = cardPanNumber[i] - '0';
                else
                   return new ValidationResult("incorrect symbol");
            }
           if(IsLunaTestPassedOk(cardPanNumbers))
            {
                return ValidationResult.Success;
            }
           return new ValidationResult("Luna test is not passed");
        }
        private bool IsLunaTestPassedOk(int[] digitArray)
        {
            int checkingDigit = digitArray[panLength - 1];
            int sum = 0;
            for (int i = 0; i < panLength - 1; i++)
            {
                if ((i + 1) % 2 != 0)
                {
                    var multiplTwoNumber = digitArray[i] * 2;
                    if (multiplTwoNumber > numberNine)
                        sum += multiplTwoNumber - numberNine;
                    else
                        sum += multiplTwoNumber;
                }
                else
                {
                    sum += digitArray[i];
                }
            }
            return ((sum + checkingDigit) % 10 == 0) ? true : false;
        }
    }
}
