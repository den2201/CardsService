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
        public static bool IsCardExpireDateValidateTrue(Card card)
        {
            if (card != null)
            {
                if (((card.Expire.Month < 1) || (card.Expire.Month > 12)) ||
                 ((card.Expire.Year < 2000) || (card.Expire.Year < DateTime.Now.Year)))
                    return false;
                return true;
            }
            return false;
        }

        public static bool IsCardPanValidateTrue(Card card)
        {
            if ((card == null) || (string.IsNullOrEmpty(card.Pan)) || (card.Pan.Length != 16))
                return false;
            if (card.Pan.Where(x => char.IsWhiteSpace(x) || char.IsLetter(x)).Any())
                return false;
            
            for( int i = 0; i < panLength; i++)
            {
                if (char.IsDigit(card.Pan[i]))
                    cardPanNumbers[i] = card.Pan[i] - '0';
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
