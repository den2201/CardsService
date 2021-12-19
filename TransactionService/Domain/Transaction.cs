using System;

namespace TransactionService.Domain
{
    public class Transaction
    { 
            public Guid Id { get; set; }

            public string TransactionName { get; set; }

            public float Amount { get; set; }

            public Guid CardId { get; set; }

            public DateTime CreatedDate { get; set; }

            public Guid UserId { get; set; }

            public string CardName { get; set;  }


    }
}
