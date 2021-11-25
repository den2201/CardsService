using System;

namespace CardService.Models.Request
{
    /// <summary>
    /// Small Dto card model for card name changing
    /// </summary>
    public class CardUpdateNameData
    {
        public Guid cardId { get; set; }
        public string cardNewName { get; set; }
    }
}
