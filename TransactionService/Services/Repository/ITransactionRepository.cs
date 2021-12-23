using SharedEntities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionService.Domain;

namespace TransactionService.Services.Repository
{
    public interface ITransactionRepository
    {
        Task<Transaction> GetById(Guid id);
        
        Task<IEnumerable<Transaction>> GetAll();

        Task<bool> Add(Guid cardId, string name, float amount, CardDto cardInfo);


        Task<bool> AddTransactioForAllUsers(string name, float amount);

        Task DeleteTransaction(Guid transactionId);

    }
}
