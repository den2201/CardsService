using CardService.Domain;
using CardService.Services.Repository;
using MassTransit;
using SharedEntities.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CardService.Consumers
{
    public class TransactionConsumer : IConsumer<CardDto>
    {
        private readonly ICardRepository _cardRepository;
        public TransactionConsumer(ICardRepository repository)
        {
            _cardRepository = repository;
        }

        public async Task Consume(ConsumeContext<CardDto> context)
        {
            await Console.Out.WriteLineAsync(context.Message.CardName);
            var card = context.Message;
            await _cardRepository.Add(new Card()
            {
                Id = card.Id.Value,
                CardName = card.CardName,
                Pan = card.Pan,
                CVC = card.CVC,
                CardDateExpired = new CardDateExpired() { Year = card.Date.Year, Month = card.Date.Month },
                IsDefault = card.IsDefault,
                UserId = card.UserId,
            });
        }
    }

   
}
