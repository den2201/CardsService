using System;
using System.ComponentModel.DataAnnotations;

namespace TransactionService.DTO
{
    public class TransactionByCardId
    {
        [Required]
        public Guid CardId { get; set; }
        [Required]
        public string TransactionName { get; set;}
        [Required]
        public float Amount { get; set; }

    }
}
