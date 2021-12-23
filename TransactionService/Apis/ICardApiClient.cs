using SharedEntities.Models;
using System;
using System.Threading.Tasks;

namespace TransactionService.Apis
{
    public interface ICardApiClient
    {
        Task<CardDto> GetCardInfo(Guid id);
        Task<CardDto> GetDefaultCardByUserId(Guid userId);
    }
}
