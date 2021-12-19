using CardService.Domain;
using CardService.Models;
using CardService.Models.Request;
using CardService.Services.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CardService.Controllers
{

    [ApiController]
    [Route("api/transaction")]
    public class TransactionController : ControllerBase
    {
        private readonly ICardRepository _repository;

        public TransactionController(ICardRepository repository)
        {
            _repository = repository;
        }


        /// <summary>
        /// Creates transaction using card id. Params: card id, transaction name, amount
        /// </summary>
        /// <returns>200OK: if card transaction addded into history table</returns>
        /// <remarks>
        /// test card id is c937c286-2522-4dfb-99bd-94d9f7f7e04b
        /// Response: only 200OK status if transaction is added into history table
        /// </remarks>
        /// <response code="200">card name is updated</response>
        /// <resposne code="404">card is not updated</resposne>

        [HttpPost("cardId")]
        [ProducesResponseType(typeof(ApiResponseModel<Card>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseModel<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseModel<ErrorMessage>), StatusCodes.Status404NotFound)]
     
        public async Task<ActionResult> AddWithCard([FromBody] Transaction transaction)
        {
            bool IsTransactionAdded = false;

             IsTransactionAdded = await _repository.AddTransactionByCardId(transaction.ItemId.Value, transaction.TransactionName, transaction.Amount);
            if(IsTransactionAdded)
            {
                return await Task.FromResult(Ok(new ApiResponseModel<DBNull>
                {
                    IsOkStatus = true
                }));
            }
            return await Task.FromResult(NotFound(new ApiResponseModel<ErrorMessage>
            {
                IsOkStatus = false,
                Data = new ErrorMessage { Code = Code.TransactionErrror, Message = "Transaction is not created. Data is not found by id" }
            }));
        }

        /// <summary>
        /// Creates transaction for all users with default cards. Params: transaction name, amount
        /// </summary>
        /// <param name="name"></param>
        /// <param name="amount"></param>
        /// <returns>200OK:  card transaction addded into history table</returns>
        /// <remarks>
        /// Response: only 200OK status if transaction is added into history table
        /// </remarks>
        /// <response code="200">transaction is created</response>
        /// <resposne code="404">transaction is not created</resposne>

        [HttpPost("allusers")]
        [ProducesResponseType(typeof(ApiResponseModel<Card>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseModel<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseModel<ErrorMessage>), StatusCodes.Status404NotFound)]

        public async Task<ActionResult> CreateTransactionAllCards([FromBody] Tuple<string, float> transactionData)
        {
            bool IsTransactionAdded = false;

            IsTransactionAdded = await _repository.AddTransactioAllUsers(transactionData.Item1, transactionData.Item2);
            if (IsTransactionAdded)
            {
                return await Task.FromResult(Ok(new ApiResponseModel<DBNull>
                {
                    IsOkStatus = true
                }));
            }
            return await Task.FromResult(NotFound(new ApiResponseModel<ErrorMessage>
            {
                IsOkStatus = false,
                Data = new ErrorMessage { Code = Code.TransactionErrror, Message = "Transaction is not created. Data is not found by id" }
            }));
        }

        /// <summary>
        /// Creates transaction using new added card. Params: CardDto, transaction name, amount
        /// </summary>
        /// <param name="Card"></param>
        /// <param name="name"></param>
        /// <param name="amount"></param>
        /// <returns>200OK:  card transaction addded into history table</returns>
        /// <remarks>
        /// Response: only 200OK status if transaction is added into history table
        /// </remarks>
        /// <response code="200">transaction is created</response>
        /// <resposne code="404">transaction is not created</resposne>

        [HttpPost("newcard")]
        [ProducesResponseType(typeof(ApiResponseModel<Card>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseModel<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseModel<ErrorMessage>), StatusCodes.Status404NotFound)]

        public async Task<ActionResult> CreateTransactionByNewCard([FromBody] TransactionNewCard transaction)
        {
            bool IsTransactionAdded = false;
            try
            {
                
                var newCardId = Guid.NewGuid();
                await _repository.Add(new Card
                {
                    Id = newCardId,
                    CardName = transaction.Card.CardName,
                    Pan = transaction.Card.Pan,
                    CVC = transaction.Card.CVC,
                    CardDateExpired = new CardDateExpired() { Year = transaction.Card.Date.Year, Month = transaction.Card.Date.Month },
                    IsDefault = transaction.Card.IsDefault,
                    UserId = transaction.Card.UserId,
                });

                    IsTransactionAdded = await _repository.AddTransactionByCardId(newCardId, transaction.TransactionName, transaction.Amount);
                if (IsTransactionAdded)
                {
                    return await Task.FromResult(Ok(new ApiResponseModel<DBNull>
                    {
                        IsOkStatus = true
                    }));
                }
                return await Task.FromResult(NotFound(new ApiResponseModel<ErrorMessage>
                {
                    IsOkStatus = false,
                    Data = new ErrorMessage { Code = Code.TransactionErrror, Message = "Transaction is not created" }
                }));
            }
          
            catch
            {
                return await Task.FromResult(NotFound(new ApiResponseModel<ErrorMessage>
                {
                    IsOkStatus = false,
                    Data = new ErrorMessage { Code = Code.TransactionErrror, Message = "Transaction is not created" }
                }));
            }
        }

    }
}
