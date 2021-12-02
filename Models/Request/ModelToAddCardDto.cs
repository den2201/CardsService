using CardService.Domain;
using CardService.Utils.Validators.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace CardService.Models.Request
{
    /// <summary>
    /// DTO Card model without some Properties
    /// </summary>
    public class ModelToAddCardDto
    {
        public Guid UserId { get; set; }

        [StringLength(3)]
        public string CVC { get; set; }

        [PanValidator(ErrorMessage = "Pan validation error")]
        [StringLength(16)]
        public string Pan { get; set; }

        public bool IsDefault { get; set; }

        [CardDateValidator(ErrorMessage = "Card date validation error")]
        public CardDate Date { get; set; }
        
        public string CardName { get; set; }
    }

    public class CardDate
    {
       public int Month  { get; set; }
       public int Year { get; set; }
    }
}
