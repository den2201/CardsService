using CardService.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace CardService.Controller
{
    [ApiController]
    [Route("api/{controller}")]
    public class CardController : ControllerBase
    {
        private readonly ICardRepository _repository;

        public CardController(ICardRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult GetCardByUserId([FromQuery]Guid userid)
        {
            return Ok(JsonConvert.SerializeObject(_repository.GetCardsByUserId(userid)));
        }
    }
}
