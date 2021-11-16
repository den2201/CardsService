using CardService.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardService.Services
{
    public interface ICardRepository
    {
        void AddCard(Card card);

       IEnumerable<Card> GetCardsByUserId(Guid userId);
    }
}
