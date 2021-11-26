using CardService.Domain;
using CardService.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CardService.BackgroundTasks
{
    public class DbCardsValidationService : IHostedService
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
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                // loop until a cancalation is requested
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                       var cards =  _cardRepository.GetAllCards();
                        foreach (var card in cards)
                        {
                            if ((card.Year < DateTime.Now.Year) || 
                            (card.Year == DateTime.Now.Year && card.Month < DateTime.Now.Month))
                                card.CardName = "Not Valid";
                        }
                        await Task.Delay(TimeSpan.FromSeconds(20), cancellationToken);
                    }
                    catch (OperationCanceledException) { }
                }
            }, cancellationToken);

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
