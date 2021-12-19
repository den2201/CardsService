using SharedEntities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionService.Datalayer;
using TransactionService.Domain;

namespace TransactionService.Services.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _appDbContext;
        public TransactionRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> Add(Guid cardId, string name, float amount, CardDto cardInfo)
        {
            var transaction = new Transaction { Id = Guid.NewGuid(), CardId = cardId, 
                Amount = amount, 
                TransactionName = name,
                CardName = cardInfo.CardName, 
                CreatedDate = DateTime.UtcNow, 
                UserId = cardInfo.UserId
                };
            await _appDbContext.AddAsync(transaction);
            if (_appDbContext.ChangeTracker.HasChanges())
            {
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public Task<bool> AddTransactioForAllUsers(string name, float amount)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteTransaction(Guid transactionId)
        {
           _appDbContext.Transactions.Remove(new Transaction { Id = transactionId });
           await _appDbContext.SaveChangesAsync();
        }

        public Task<IEnumerable<Transaction>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> GetById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
