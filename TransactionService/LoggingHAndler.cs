using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TransactionService
{
    internal class LoggingHAndler : DelegatingHandler
    {
        private readonly IHttpContextAccessor contextAccessor;

        public LoggingHAndler(IHttpContextAccessor httpContextAccessor)
        {
            contextAccessor = httpContextAccessor;
        }
       protected override Task<HttpResponseMessage> SendAsync (HttpRequestMessage requestMessage, CancellationToken token)
        {
            var requestId = contextAccessor.HttpContext?.Items["Request_ID"] as string;
            requestMessage.Headers.Add("Request_ID", requestId);
            return base.SendAsync(requestMessage, token);
        }
    }
}