using CardService.Domain;
using CardService.Models.Request;
using Microsoft.EntityFrameworkCore;
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

        public async Task Add<T>(T entity) where T : IEntity
        {
            await _appDbContext.AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<bool> AddTransactionByCardId(Guid guid, string name, float amount)
        {
            bool IsTransactionAdded = false;
            var cards = _appDbContext.Cards.Where(x => x.Id == guid);
           
            if (cards.Any())
            {
                var card = cards.First();
               await _appDbContext.TransactionHistory.AddAsync(new TransactionHistory
                {
                    Id = Guid.NewGuid(),
                    CardId = card.Id,
                    Amount = amount,
                    TransactionName = name
                });
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            
            return false;
        }

        public async Task<bool> AddTransactioAllUsers(string name, float amount)
        {
            var defaultCardsIds =  _appDbContext.Cards.Where(x => x.IsDefault == true).Select(x => x.Id);
            if (defaultCardsIds is not null)
            {
                foreach (var cardId in defaultCardsIds)
                {
                    await _appDbContext.TransactionHistory.AddAsync(new TransactionHistory
                    {
                        Id = Guid.NewGuid(),
                        CardId = cardId,
                        Amount = amount,
                        TransactionName = name
                    });
                }
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> Delete<T>(T enity) where T : IEntity
        {
            try
            {
                _appDbContext.Remove(enity);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Card> GetById(Guid id)
        {
            return await _appDbContext.Cards.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Card>> GetByUserId(Guid userId)
        {
            return await _appDbContext.Cards.Where(x => x.UserId == userId).Include(m => m.CardDateExpired).ToListAsync();
        }

        public async Task<bool> Update(Guid cardId, string newCardName)
        {
            try
            {
                var card = _appDbContext.Cards.FirstOrDefault(x => x.Id == cardId);
                if (card is not null)
                {
                    card.CardName = newCardName;
                    _appDbContext.Update(card);
                    await _appDbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

    }
}
