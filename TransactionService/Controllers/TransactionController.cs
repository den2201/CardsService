using IdentityModel.Client;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SharedEntities.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TransactionService.Apis;
using TransactionService.DTO;
using TransactionService.Services;
using TransactionService.Services.Repository;

namespace TransactionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly ITransactionRepository repository;
        private readonly ILogger<TransactionController> logger;
        private readonly ICardApiClient cardApiClient;

        public TransactionController(IBus bus, ITransactionRepository transactionRepository, ICardApiClient client, ILogger<TransactionController> logger)
        {
            _bus = bus;
            repository = transactionRepository;
            cardApiClient = client;
            this.logger = logger;
        }

        /// <summary>
        /// create transaction by new card
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>

        [HttpPost("create/newcard")]
        [ProducesResponseType(typeof(ApiResponseModel<DBNull>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseModel<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseModel<ErrorMessage>), StatusCodes.Status404NotFound)]


        public async Task<ActionResult> CreateTransactionByNewCard(TransactionNewCard transaction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    logger.LogInformation($"Begin Action : {nameof(TransactionController)} {nameof(CreateTransactionByNewCard)} {DateTime.Now}");
                    var document = cardApiClient.GetAuthorization();
                    var cardId = Guid.NewGuid();
                   transaction.Card.Id = cardId;
                    if (await repository.Add(cardId, transaction.TransactionName, transaction.Amount, transaction.Card))
                    {
                       await _bus.Publish(transaction.Card);
                        logger.LogInformation($"OK result Action : {nameof(TransactionController)} {nameof(CreateTransactionByNewCard)} {DateTime.Now}");
                        return Ok(new ApiResponseModel<DBNull>() { IsOkStatus = true, Message = "Transaction was created" });
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError($"{nameof(TransactionController)} {nameof(CreateTransactionByNewCard)} {DateTime.Now}" + ex.Message);
                    return NotFound(new ApiResponseModel<ErrorMessage>() { IsOkStatus = false, Data = new ErrorMessage { Code = Code.TransactionError, Message = ex.Message } });
                }
            }
            logger.LogError($"{nameof(TransactionController)} {nameof(CreateTransactionByNewCard)} {DateTime.Now}" + $"error code: {Code.IncorrectRequestData}");
            return BadRequest(new ApiResponseModel<ErrorMessage>() { IsOkStatus = false, Data = new ErrorMessage { Code = Code.IncorrectRequestData, Message = "Invalid request params" } });
        }

        /// <summary>
        /// create transaction by existing card
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreateByCardId(TransactionByCardId transaction)
        {
            logger.LogInformation($"Begin Action : {nameof(TransactionController)} {nameof(TransactionByCardId)} {DateTime.Now}");
            var client = new HttpClient();
         
           

            var cardData = await cardApiClient.GetCardInfo(transaction.CardId);
            if(cardData is not null)
            {
                await repository.Add(transaction.CardId, transaction.TransactionName, transaction.Amount, cardData);
                logger.LogInformation($"OK result of Action : {nameof(TransactionController)} {nameof(TransactionByCardId)} {DateTime.Now}");
                return Ok(new ApiResponseModel<DBNull>() { IsOkStatus = true, Message = "Transaction commited" });
            }
            logger.LogError($"{nameof(TransactionController)} {nameof(TransactionByCardId)} {DateTime.Now}" + $"error code: {Code.TransactionError}");
            return BadRequest(new ApiResponseModel<ErrorMessage> { 
                IsOkStatus = false, 
                Data = new ErrorMessage 
                { Code = Code.TransactionError, 
                    Message = "Error transaction commit" } 
            });
        }

        /// <summary>
        /// Create Transaction by user's default card
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create/default")]
        public async Task<ActionResult> CreateByUsersDefaultCard(TransactionDefaultCard transaction)
        {
            logger.LogInformation($"Begin Action : {nameof(TransactionController)} {nameof(CreateByUsersDefaultCard)} {DateTime.Now}");
            var client = new HttpClient();
            var discoveryDocoment = await client.GetDiscoveryDocumentAsync("https://localhost:10001");
            var cardData = await cardApiClient.GetDefaultCardByUserId(transaction.UserId);
            if (cardData is not null)
            {
                await repository.Add(cardData.Id.Value, transaction.TransactionName, transaction.Amount, cardData);
                logger.LogInformation($"OK result of Action : {nameof(TransactionController)} {nameof(CreateByUsersDefaultCard)} {DateTime.Now}");
                return Ok(new ApiResponseModel<DBNull>() { IsOkStatus = true, Message = "Transaction commited" });
            }
            logger.LogError($"{nameof(TransactionController)} {nameof(TransactionByCardId)} {DateTime.Now}" + $"error code: {Code.TransactionError}");
            return BadRequest(new ApiResponseModel<ErrorMessage>
            {
                IsOkStatus = false,
                Data = new ErrorMessage
                {
                    Code = Code.TransactionError,
                    Message = "Error transaction commit"
                }
            });
        }

    }

}
