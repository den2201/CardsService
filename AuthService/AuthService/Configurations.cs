using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace AuthService
{
    public static class Configurations
    {
        public static IEnumerable<Client> GetClients() =>

              new List<Client>
             {
                 new Client()
                 {
                     ClientId = "client_card",
                     ClientSecrets = {new Secret("client_secret".ToSha256()) },
                     AllowedGrantTypes = GrantTypes.ClientCredentials,
                     AllowedScopes =
                     {
                         "CardAPI", "TransactionAPI"
                     }
                 },
                  new Client()
                 {
                     ClientId = "client_transaction",
                     ClientSecrets = {new Secret("client_secret".ToSha256()) },
                     AllowedGrantTypes = GrantTypes.ClientCredentials,
                     AllowedScopes =
                     {
                         "TransactionAPI"
                     }
                 }

             };
        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>()
            {
                new ApiResource("CardAPI"),
                new ApiResource("TransactionAPI")
            };

        public static IEnumerable<IdentityResource> GetIdentityResource() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId()
            };
    }
}
