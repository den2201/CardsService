namespace CardService.Models
{
    public class ErrorMessage
    {
        public Code Code { get; set; }
        public string Message { get; set; }
    }

    public enum Code
    {
        IncorrectRrequestData = 1,
        UpdateCardNameError,
        CardNotFound,
        CardDeleteError,
        CardAddingError
    }
       
}
