using CardService.Domain;
using CardService.Services;
using CardService.Services.Repository;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CardService.BackgroundTasks
{
    public class DbCardsValidationService : BackgroundService
    {
        private readonly ICardRepository _cardRepository;

        /// <summary>
        /// simple service checks date of card for expiring
        /// </summary>
        /// <param name="repository"></param>
        public DbCardsValidationService(ICardRepository repository)
        {
            _cardRepository = repository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                }
                catch (OperationCanceledException) { }
            }
        }
    }
}

