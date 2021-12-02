using CardService.Domain;

namespace CardService.Utils
{
    public class InvisibleParamsCardPtototypeCreater
    {
        public Card Create(Card entity)
        {
            return new Card
            {
                CVC = HideCvcCode(entity.CVC),
                Pan = HidePanCode(entity.Pan),
                Date = entity.Date,
                CardName = entity.CardName,
                Id = entity.Id,
                IsDefault = entity.IsDefault,
                UserId = entity.UserId
            };
        }
        
        private string HidePanCode(string code)
        {
            char[] charArray = code.ToCharArray();
            for(int i = 0; i < charArray.Length - 3; i++)
            {
                if(char.IsDigit(charArray[i]))
                {
                    charArray[i] = 'X';
                }
            }
            return new string(charArray);
        }
        private string HideCvcCode(string cvcCode)
        {
            char[] charArray = cvcCode.ToCharArray();
           for (int x =0; x < charArray.Length; x++)
            {
                charArray[x] = 'X';
            }
            return new string(charArray);
        }
    }
}
