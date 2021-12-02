using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardService.Database.Entites
{
    public class CardEntity
    {
       [Column("CardId")]
       public Guid Id { get; set; }
       public Guid UserId { get; set; }
       public string CVC { get; set; }
       public string Pan { get; set; }
       public CardDateEntity Date { get; set; }
       public bool IsDefault { get; set; }
       public string CardName { get; set; }
      
    }
}
