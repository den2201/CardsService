using CardService.Domain;
using CardService.Filters;
using CardService.Models;
using CardService.Models.Request;
using CardService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet("getbyuserid")]
        [ProducesResponseType(typeof(Card), 200)]
        public ActionResult GetCardByUserId([FromQuery] Guid userid)
        {
            var cards = _repository.GetCardsByUserId(userid);
            if((cards == null) || (cards.Count == 0))
            {
                return NotFound(new ApiResponseModel { IsOkStatus = false, 
                                    ErrorMessage = new ErrorMessage{ Code = Code.CardNotFound, 
                                                                                Message ="Not found" } });
            }
            var response = new ApiResponseModel { Data = cards, IsOkStatus = true };
            var jsonString = JsonConvert.SerializeObject(response);
            return Ok(jsonString);
        }


        [HttpPost("addcard")]
        [ServiceFilter(typeof(CardDataValidatorFilter))]
        public ActionResult AddCard([FromBody] ModelToAddCardDto card)
        {
            if (!ModelState.IsValid)
            {
                List<string> err = new ();
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in allErrors)
                {
                    err.Append(error.ErrorMessage);
                }
                return BadRequest(new ApiResponseModel { IsOkStatus = false, ErrorMessage = new ErrorMessage { Code = Code.IncorrectRrequestData, Message = err.ToString() } });
            }

            if (card == null)
                return BadRequest(new ApiResponseModel
                {
                    IsOkStatus = false,
                    ErrorMessage = new ErrorMessage { Code = Code.IncorrectRrequestData, Message = "Incorrect input data" },
                    Data = card
                });

            try
            {
                _repository.AddCard(new Card
                {
                    Id = new Guid(),
                    UserId = card.UserId,
                    CardName = card.CardName,
                    CVC = card.CVC,
                    Month = card.Month, Year = card.Year,
                    IsDefault = card.IsDefault,
                    Pan = card.Pan
                });
                return Ok(new ApiResponseModel { IsOkStatus = true});
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponseModel { IsOkStatus = false, ErrorMessage = new ErrorMessage
                {
                    Code = Code.CardAddingError,
                    Message = ex.Message
                } });
            }
        }

        [HttpPost("updatename")]
        public ActionResult UpdateCardName([FromBody] CardUpdateNameData data)
        {
           
            var updatedCard = _repository.UpdateCardName(data.cardId, data.cardNewName);

            if (updatedCard != null)
                return Ok(new ApiResponseModel { IsOkStatus = true, Data = updatedCard });
            else
                return NotFound(new ApiResponseModel
                {
                    IsOkStatus = false,
                    ErrorMessage = new ErrorMessage { Code = Code.UpdateCardNameError, Message = "Updating Error" },
                });
        }

        [HttpPost("delete")]
        public ActionResult DeleteCard([FromBody] Guid card)
        {
            if (_repository.DeleteCard(card))
                return Ok(new ApiResponseModel { IsOkStatus = true });

            return BadRequest(new ApiResponseModel
            {
                IsOkStatus = false,
                ErrorMessage = new ErrorMessage { Code = Code.CardDeleteError, Message = "Card delete Error" }
            });
        }

        private IActionResult RequestDataIncorrectReturnError()
        {
            return BadRequest(new ApiResponseModel
            {
                IsOkStatus = false,
                ErrorMessage = new ErrorMessage { Code = Code.IncorrectRrequestData, Message = "Incorrect input data" },
            });
        }
    }
}
