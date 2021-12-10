using System;

namespace CardService.Domain
{
    public class TransactionHistory
    {
        public Guid Id { get; set; }

        public string TransactionName { get; set; }
        public float Amount { get; set; }

        public Guid CardId { get; set; }

        public Card Card { get; set; }

      
    }
}
