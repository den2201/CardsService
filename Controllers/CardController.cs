using CardService.Domain;
using CardService.Filters;
using CardService.Models;
using CardService.Models.Request;
using CardService.Services;
using CardService.Services.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Test methos Todo: delete
        /// </summary>
        /// <returns></returns>
        [HttpGet("test")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public  string Test()
        { 
            return "hello";
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
        public async Task<ActionResult> GetCardByUserId([FromRoute] Guid userid)
        {
          Dictionary<Guid,CardDto> cardDtos = new(); ;
            var cards = await _repository.GetByUserId(userid) as List<Card>;
            foreach( var card in cards)
            {
                cardDtos.Add(card.Id,new CardDto
                {
                    Pan = card.Pan,
                    CardName = card.CardName,
                    CVC = card.CVC,
                    Date = new() { Month = card.CardDateExpired.Month, Year = card.CardDateExpired.Year },
                    IsDefault = card.IsDefault,
                    UserId = card.UserId
                });
            }
            if ((cards == null) || (cards.Count == 0))
            {
                return await Task.FromResult<ActionResult>(NotFound(new ApiResponseModel<ErrorMessage>
                {
                    IsOkStatus = false,
                    Data =
                    new ErrorMessage
                    {
                        Code = Code.CardNotFound,
                        Message = "Not found"
                    }
                }));
            }
            var response = new ApiResponseModel<Dictionary<Guid,CardDto>> { Data = cardDtos, IsOkStatus = true };
            var jsonString = JsonConvert.SerializeObject(response);
            return await Task.FromResult<ActionResult>(Ok(jsonString));
        }


        /// <summary>
        /// adds new card
        /// </summary>
        /// <param name="card"></param>
        /// <returns>Response model with flag of OK or Bad request result</returns>
        /// <remarks>
        /// Data for test:
        /// user id : 3bad8330-d287-4319-bb3f-1f9be9331814
        /// valid card pan : 4397185796979658
        /// </remarks>
        /// <response code="200">card is added</response>
        /// <resposne code="400">request error</resposne>
        /// <resposne code="404">card is not added error</resposne>

        [HttpPost("add")]
        [ProducesResponseType(typeof(ApiResponseModel<DBNull>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseModel<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseModel<ErrorMessage>), StatusCodes.Status404NotFound)]
        
        public async Task<ActionResult> AddCard([FromBody] CardDto card)
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
                return BadRequest(new ApiResponseModel<ErrorMessage> { IsOkStatus = false, Data = new ErrorMessage { Code = Code.IncorrectRequestData, Message = errors.ToString() } });
            }

            if (card == null)
                return BadRequest(new ApiResponseModel<ErrorMessage>
                {
                    IsOkStatus = false,
                    Data = new ErrorMessage { Code = Code.IncorrectRequestData, Message = "Incorrect input data" },
                });

            try
            {
               await _repository.Add(new Card
                {
                    Id = Guid.NewGuid(),
                    CardName = card.CardName,
                    Pan = card.Pan,
                    CVC = card.CVC,
                    CardDateExpired = new CardDateExpired() { Year = card.Date.Year, Month = card.Date.Month },
                    IsDefault = card.IsDefault,
                    UserId = card.UserId,

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

        [HttpPost("user/update/card/{cardId}/{newName}")]
        [ProducesResponseType(typeof(ApiResponseModel<Card>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseModel<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseModel<ErrorMessage>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateCardName([FromRoute] Guid cardId, string newName)
        {
           var isNameUpdated = await _repository.Update(cardId, newName);
            
           if (isNameUpdated)
                return Ok(new ApiResponseModel<DBNull>{ IsOkStatus = true});
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
        public async Task<ActionResult> DeleteCard([FromRoute] Guid cardid)
        {
            var card = await _repository.GetById(cardid);
            if (card is not null)
            {
                await _repository.Delete(card);
                return  Ok(new ApiResponseModel<DBNull> { IsOkStatus = true });
            }

            return  BadRequest(new ApiResponseModel<ErrorMessage>
            {
                IsOkStatus = false,
                Data = new ErrorMessage { Code = Code.CardDeleteError, Message = "Card delete Error" }
            });
        }
    }
}
