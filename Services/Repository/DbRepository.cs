using CardService.Database.Entites;
using CardService.Domain;
using CardService.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardService.Services.Repository
{
    public class DbRepository : ICardRepository
    {
       private readonly AppDbContext _appDbContext;
        public DbRepository(AppDbContext context)
        {
            _appDbContext = context;
        }

        public async void AddCard(ModelToAddCardDto cardDto)
        {
            var card = new CardEntity
            {
                Id =  Guid.NewGuid(),
                Pan = cardDto.Pan,
                CardName = cardDto.CardName,
                CVC = cardDto.CVC,
                IsDefault = cardDto.IsDefault,
                UserId = cardDto.UserId,
            };
            await _appDbContext.Cards.AddAsync(card);
            var cardReleaseDate = new CardDateEntity
            {
                CardId = card.Id,
                Month = cardDto.Date.Month,
                Year = cardDto.Date.Year
            };

            await _appDbContext.ExpiredDates.AddAsync(cardReleaseDate);
             _appDbContext.SaveChangesAsync();
            
        }
                
        public bool DeleteCard(Guid cardId)
        {
            var card = _appDbContext.Cards.FirstOrDefault(x => x.Id == cardId);
            if(card != null)
            {
                _appDbContext.Cards.Remove(card);
                _appDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public IEnumerable<Card> GetAllCards()
        {
            var cards = from card in _appDbContext.Cards
                        join date in _appDbContext.ExpiredDates on card.Id equals date.CardId
                        select new Card
                        {
                            Id = card.Id,
                            Pan = card.Pan,
                            CardName = card.CardName,
                            CVC = card.CVC,
                            UserId = card.UserId,
                            Date = new CardDateExpired { Year = date.Year, Month = date.Month }
                        };
            return cards.ToList();
        }
               

        public List<Card> GetCardsByUserId(Guid userId)
        {
            var result = from card in _appDbContext.Cards where card.UserId == userId
                         join date in _appDbContext.ExpiredDates on card.Id equals date.CardId
                        
                         select new Card
                         {
                             Id=card.Id,
                             Pan = card.Pan,
                             CardName=card.CardName,
                             CVC = card.CVC,
                             UserId = card.UserId,
                             Date = new CardDateExpired { Year = date.Year, Month = date.Month}
                         };

            return result.ToList();
        }

        public bool UpdateCardName(Guid userId, Guid cardId, string cardName)
        {
            var updatedCard = _appDbContext.Cards.FirstOrDefault(x => x.UserId == userId && x.Id == cardId);
          
            if (updatedCard != null)
            {
                updatedCard.CardName = cardName;
                _appDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
