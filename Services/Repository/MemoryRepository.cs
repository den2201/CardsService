using CardService.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardService.Services.Repository
{
    public class MemoryRepository : ICardRepository
    {
        private  List<Card> _cards;

        public MemoryRepository()
        {
            _cards = new List<Card>()
            {
                new Card(){ UserId = new Guid("3bad8330-d287-4319-bb3f-1f9be9331814"),
                    CVC = "123",
                    Pan = "4397 1857 9697 9658",
                    IsDefault = true,
                    CardName = "First card",
                    Expire = new Expire(11, 2030)
                },
                 new Card(){ UserId = new Guid("3bad8330-d287-4319-bb3f-1f9be9331814"),
                    CVC = "666",
                    Pan = "2367 0000 0001 9658",
                    IsDefault = false,
                    CardName = "Second card",
                    Expire = new Expire(5, 2040)
                },
            };
        }
        public  void  AddCard(Card card)
        {
            _cards.Add(new Card { UserId = card.UserId, 
                CardName = card.CardName, 
                CVC = card.CVC,
                Expire = card.Expire, 
                IsDefault = card.IsDefault, 
                Pan = card.Pan 
            });
        }

        public IEnumerable<Card> GetCardsByUserId(Guid userId)
        {
            return _cards.Where(x => x.UserId == userId).ToList();
        }
    }
}
