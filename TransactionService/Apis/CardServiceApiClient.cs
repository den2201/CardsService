using IdentityModel.Client;
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
            await Authorize();
                var cardDto = await _cardApiClient.GetFromJsonAsync<CardDto>($"api/cards/{id}");
                return cardDto ?? new CardDto();
        }

       public async Task<CardDto> GetDefaultCardByUserId(Guid userid) 
        {
            await Authorize();
            var cardDto = await _cardApiClient.GetFromJsonAsync<CardDto>($"api/cards/default/{userid}");
            return cardDto ?? new CardDto();
        }

        private async Task Authorize()
        {
            var discoveryDocoment = await _cardApiClient.GetDiscoveryDocumentAsync("https://localhost:10001");
            var tokenResponse = await _cardApiClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocoment.TokenEndpoint,
                ClientId = "client_card",
                ClientSecret = "client_secret",
                Scope = "CardAPI"
            });
            _cardApiClient.SetBearerToken(tokenResponse.AccessToken);
        }

        public async Task<DiscoveryDocumentResponse> GetAuthorization()
        {
            var discoveryDocoment = await _cardApiClient.GetDiscoveryDocumentAsync("https://localhost:10001");
            return discoveryDocoment;
        }

    }


}
