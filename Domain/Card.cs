using System;
using System.ComponentModel.DataAnnotations;

namespace CardService.Domain
{
    public class Card
    {
        public Guid UserId { get; set; }
        public string CVC { get; set; }
        public string Pan { get; set; }
        public Expire Expire { get; set; }
        public bool IsDefault { get; set; }
        public string CardName { get; set; }

    }
    public struct Expire
    {
       public Expire(int month, int year)
        {
            Month = month;
            Year = year;
        }
        public int Month;
        public int Year;
       
    }
}
