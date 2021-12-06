using CardService.Services.Repository;
using CardService.Utils.Validators.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardService.Domain
{
    /// <summary>
    /// Domain card Model with full options
    /// </summary>
    public class Card : IEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string CVC { get; set; }
        public string Pan { get; set; }
        public bool IsDefault { get; set; }
        public string CardName { get; set; }
        public CardDateExpired CardDateExpired { get; set; }

    }

    public class CardDateExpired
    {
        public int Year { get; set; }
        public  int Month { get; set; }

        public Card Card { get; set; }
        public Guid CardId { get; set; }
    }
}
