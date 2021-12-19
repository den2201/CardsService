using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SharedEntities.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TransactionService.Services;

namespace TransactionService.Apis
{
    public class CardServiceApiClient : ICardApiClient
    {

        private readonly HttpClient _cardApiClient;

        public CardServiceApiClient(HttpClient client)
        {
            _cardApiClient = client;
        }

        public async Task<CardDto> GetCardInfo(Guid id)
        {
            
                var cardDto = await _cardApiClient.GetFromJsonAsync<CardDto>($"api/cards/{id}");
                return cardDto;

        }

       public async Task<CardDto> GetDefaultCardByUserId(Guid userid)
        {
            var cardDto = await _cardApiClient.GetFromJsonAsync<CardDto>($"api/cards/default/{userid}");
            return cardDto;
        }
    }


}
