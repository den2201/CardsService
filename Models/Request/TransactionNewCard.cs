using System.ComponentModel.DataAnnotations;

namespace CardService.Models.Request
{
    public class TransactionNewCard
    {
        [Required]
        public CardDto Card { get; set; }
        
        [Required]
        public string TransactionName { get; set; }

        [Required]
        public float Amount { get; set; }
    }
}
