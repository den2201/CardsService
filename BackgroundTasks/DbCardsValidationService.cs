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
                    //var cards = _cardRepository.GetAllCards();
                    //foreach (var card in cards)
                    //{
                    //    if ((card.Date.Year < DateTime.Now.Year) ||
                    //    (card.Date.Year == DateTime.Now.Year && card.Date.Month< DateTime.Now.Month))
                    //        card.CardName = "Not Valid";
                    //    await Task.Delay(5000, stoppingToken);
                    //}
                }
                catch (OperationCanceledException) { }
            }
        }
    }
}

