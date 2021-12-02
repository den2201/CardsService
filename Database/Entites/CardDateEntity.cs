using System;
using System.ComponentModel.DataAnnotations;

namespace CardService.Database.Entites
{
    
    public class CardDateEntity
    {

        [Key]
        public Guid CardId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public CardEntity Card { get; set; }
    }
}
