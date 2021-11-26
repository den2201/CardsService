using CardService.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardService.Services
{
    public interface ICardRepository
    {
        void AddCard(Card card);

       List<Card> GetCardsByUserId(Guid userId);
       Card UpdateCardName(Guid cardId, string cardName);
       bool DeleteCard(Guid cardId);
       IEnumerable<Card> GetAllCards();
    }
}
