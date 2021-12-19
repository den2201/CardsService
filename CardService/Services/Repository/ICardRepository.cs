using CardService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardService.Services.Repository
{
    public interface ICardRepository : IRepository
    {
        Task<Card> GetById(Guid id);
        Task<bool> Update(Guid cardId, string newCardName);
        Task<IEnumerable<Card>> GetByUserId(Guid userId);
        Task<Card> GetDefault(Guid userId);
    }
}
