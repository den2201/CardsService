using System;
using System.ComponentModel.DataAnnotations;

namespace CardService.Models.Request
{
    public class Transaction
    {
        public Guid? ItemId { get; set; }
        
        [Required]
        public string TransactionName { get; set; }
       
        [Required]
        public float Amount { get; set; }
    }
}
