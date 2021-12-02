using CardService.Domain;
using CardService.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardService.Services.Repository
{
    public class MemoryRepository : ICardRepository
    {
        private IList<Card> _cards;

        public MemoryRepository()
        {
            _cards = new List<Card>()
            {
                new Card(){
                    Id = new Guid("c937c286-2522-4dfb-99bd-94d9f7f7e04b"),
                    UserId = new Guid("3bad8330-d287-4319-bb3f-1f9be9331814"),
                    CVC = "123",
                    Pan = "4397185796979658",
                    IsDefault = true,
                    CardName = "First card",
                  
                },
                 new Card(){
                    Id = new Guid("506c2beb-92e2-47a4-acc5-e40a6c07df12"),
                    UserId = new Guid("3bad8330-d287-4319-bb3f-1f9be9331814"),
                    CVC = "666",
                    Pan = "2367000000019234",
                    IsDefault = false,
                    CardName = "Second card",
                   
                },
            };
        }
        public  void  AddCard(ModelToAddCardDto card)
        {
            _cards.Add(new Card { Id =  Guid.NewGuid(),
                UserId = card.UserId, 
                CardName = card.CardName, 
                CVC = card.CVC,
               
                IsDefault = card.IsDefault, 
                Pan = card.Pan 
            });
        }

        public bool DeleteCard(Guid cardId)
        {
            var card = _cards.FirstOrDefault(x => x.Id == cardId);
            if (card != null)
            {
                _cards.Remove(card);
                return true;
            }
            return false;
        }

        public IEnumerable<Card> GetAllCards()
        {
            return _cards;
        }

        public List<Card> GetCardsByUserId(Guid userId)
        {
            return _cards.Where(x => x.UserId == userId).ToList();
        }

        public bool UpdateCardName(Guid userID, Guid cardId, string cardName)
        {
            var card = _cards.FirstOrDefault(x => x.UserId == userID && x.Id == cardId);
            if (card != null)
            {
                card.CardName = cardName;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
