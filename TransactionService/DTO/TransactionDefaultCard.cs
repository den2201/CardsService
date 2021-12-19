using System;
using System.ComponentModel.DataAnnotations;

namespace TransactionService.DTO
{
    public class TransactionDefaultCard
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string TransactionName { get; set; }
        [Required]
        public float Amount { get; set; }
    }
}
