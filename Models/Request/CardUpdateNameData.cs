using System;

namespace CardService.Models.Request
{
    public class CardUpdateNameData
    {
        public Guid cardId { get; set; }
        public string cardNewName { get; set; }
    }
}
