using CardService.Domain;
using CardService.Models.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardService.Services
{
    public interface ICardRepository
    {
        void AddCard(ModelToAddCardDto card);

       List<Card> GetCardsByUserId(Guid userId);
       bool UpdateCardName(Guid userId,Guid cardId, string cardName);
       bool DeleteCard(Guid cardId);
       IEnumerable<Card> GetAllCards();
    }
}
