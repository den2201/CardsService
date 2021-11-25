using System;
using System.ComponentModel.DataAnnotations;

namespace CardService.Models.Request
{
    public class ModelToAddCardDto
    {
        public Guid UserId { get; set; }
        
        [StringLength(3)]
        public string CVC { get; set; }
        
        [StringLength(16)]
        public string Pan { get; set; }

        public bool IsDefault { get; set; }

        public int Month { get; set; } 
        public int Year { get; set; }

        public string CardName { get; set; }
    }
}
