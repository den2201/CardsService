using CardService.Domain;
using CardService.Filters;
using CardService.Models;
using CardService.Models.Request;
using CardService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardService.Controller
{
    [ApiController]
    [Route("api/cards")]
    public class CardController : ControllerBase
    {
        private readonly ICardRepository _repository;
    
        public CardController(ICardRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets list of cards by user id
        /// </summary>
        /// <param name="userid"></param>
        /// <returns>user cards list</returns>
        /// <remarks>
        /// test user id is 3bad8330-d287-4319-bb3f-1f9be9331814
        /// Response: two cards
        /// </remarks>
        /// <response code="200">Returns existing cards</response>
        /// <resposne code="404">no cards</resposne>

        [HttpGet("user/{userid}")]
        [ProducesResponseType(typeof(ApiResponseModel<ErrorMessage>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseModel<ErrorMessage>), StatusCodes.Status404NotFound)]
        public ActionResult GetCardByUserId([FromRoute] Guid userid)
        {
            var cards = _repository.GetCardsByUserId(userid);
            if((cards == null) || (cards.Count == 0))
            {
                return NotFound(new ApiResponseModel<ErrorMessage> { IsOkStatus = false, Data = 
                    new ErrorMessage{ Code = Code.CardNotFound, 
                                          Message ="Not found" } });
            }
            var response = new ApiResponseModel<IEnumerable<Card>> { Data = cards, IsOkStatus = true };
            var jsonString = JsonConvert.SerializeObject(response);
            return Ok(jsonString);
        }


        /// <summary>
        /// adds new card
        /// </summary>
        /// <param name="card"></param>
        /// <returns>Response model with flag of OK or Bad request result</returns>
        /// <response code="200">card is added</response>
        /// <resposne code="400">request error</resposne>
        /// <resposne code="404">card is not added error</resposne>

        [HttpPost("add")]
        [ProducesResponseType(typeof(ApiResponseModel<ModelToAddCardDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseModel<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseModel<ErrorMessage>), StatusCodes.Status404NotFound)]
        
        public ActionResult AddCard([FromBody] ModelToAddCardDto card)
        {
            if (!ModelState.IsValid)
            {
                StringBuilder errors = new StringBuilder();
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in allErrors)
                {
                    errors.Append(error.ErrorMessage);
                    errors.AppendLine();
                }
                return BadRequest(new ApiResponseModel<ErrorMessage> { IsOkStatus = false, Data = new ErrorMessage { Code = Code.IncorrectRrequestData, Message = errors.ToString() } });
            }

            if (card == null)
                return BadRequest(new ApiResponseModel<ErrorMessage>
                {
                    IsOkStatus = false,
                    Data = new ErrorMessage { Code = Code.IncorrectRrequestData, Message = "Incorrect input data" },
                });

            try
            {
                _repository.AddCard(new Card
                {
                    Id = new Guid(),
                    UserId = card.UserId,
                    CardName = card.CardName,
                    CVC = card.CVC,
                    Date =card.Date,
                    IsDefault = card.IsDefault,
                    Pan = card.Pan
                });
                return Ok(new ApiResponseModel<DBNull> { IsOkStatus = true});
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponseModel<ErrorMessage> { IsOkStatus = false, Data = new ErrorMessage
                {
                    Code = Code.CardAddingError,
                   Message = ex.Message
                }});
            }
        }

        /// <summary>
        /// Updates card name. Params: DTO model of card with card id and new card name
        /// </summary>
        /// <param name="data"></param>
        /// <returns>200OK: if card name is updated</returns>
        /// <remarks>
        /// test card id is c937c286-2522-4dfb-99bd-94d9f7f7e04b
        /// Response: only 200OK status if card name was updated or error code status
        /// </remarks>
        /// <response code="200">card name is updated</response>
        /// <resposne code="404">card is not updated</resposne>

        [HttpPost("updatename")]
        [ProducesResponseType(typeof(ApiResponseModel<Card>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseModel<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseModel<ErrorMessage>), StatusCodes.Status404NotFound)]
        public ActionResult UpdateCardName([FromBody] CardUpdateNameData data)
        {
           
            var updatedCard = _repository.UpdateCardName(data.cardId, data.cardNewName);

            if (updatedCard != null)
                return Ok(new ApiResponseModel<Card>{ IsOkStatus = true, Data = updatedCard });
            else
                return NotFound(new ApiResponseModel<ErrorMessage>
                {
                    IsOkStatus = false,
                    Data = new ErrorMessage { Code = Code.UpdateCardNameError, Message = "Updating Error" },
                });
        }
        /// <summary>
        /// deletes card by card id (Guid)
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        /// <remarks>
        /// test card id is c937c286-2522-4dfb-99bd-94d9f7f7e04b
        /// Response: only 200OK status if card was deleted or error code status
        /// </remarks>
        [HttpPost("delete/{cardid}")]
        [ProducesResponseType(typeof(ApiResponseModel<DBNull>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseModel<ErrorMessage>), StatusCodes.Status400BadRequest)]
        public ActionResult DeleteCard([FromRoute] Guid cardid)
        {
            if (_repository.DeleteCard(cardid))
                return Ok(new ApiResponseModel<DBNull> { IsOkStatus = true });

            return BadRequest(new ApiResponseModel<ErrorMessage>
            {
                IsOkStatus = false,
                Data = new ErrorMessage { Code = Code.CardDeleteError, Message = "Card delete Error" }
            });
        }
    }
}
