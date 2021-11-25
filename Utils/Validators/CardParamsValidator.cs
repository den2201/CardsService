using CardService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardService.Services.Validators
{
    public  class CardParamsValidator
    {
        const int panLength = 16;
        const int numberNine = 9;
        private static int[] cardPanNumbers = new int[panLength];
        
        public static bool IsCardExpireDateValidateTrue(int cardMonth, int cardYear)
        {
            if ((cardMonth < 1) || (cardMonth > 12) ||
                                (cardYear < DateTime.Now.Year))
                return false;
            return true;
        }
           
           

        public static bool IsCardPanValidateTrue(string cardPanNumber)
        {
            if ((string.IsNullOrEmpty(cardPanNumber) || (cardPanNumber.Length != 16)))
                return false;
            if (cardPanNumber.Where(x => char.IsWhiteSpace(x) || char.IsLetter(x)).Any())
                return false;
            
            for( int i = 0; i < panLength; i++)
            {
                if (char.IsDigit(cardPanNumber[i]))
                    cardPanNumbers[i] = cardPanNumber[i] - '0';
                else
                    return false;
            }
            return IsLunaTestPassedOk(cardPanNumbers);
        }

        /// <summary>
        /// Luna TEST
        /// </summary>
        /// <param name="LunaTest"></param>
        /// <returns></returns>
        private static bool IsLunaTestPassedOk(int[] digitArray)
        {
            int checkingDigit = digitArray[panLength - 1];
            int sum = 0;
            for(int i = 0; i < panLength-1; i++)
            {
                if((i+1)%2 != 0)
                {
                    var multiplTwoNumber = digitArray[i] *2;
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
            return ((sum + checkingDigit) % 10 == 0)  ? true : false;
        }
    }
}
