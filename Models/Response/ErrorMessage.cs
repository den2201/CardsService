namespace CardService.Models
{
    /// <summary>
    /// Error Message Model
    /// </summary>
    public class ErrorMessage
    {
        public Code Code { get; set; }
        public string Message { get; set; }
    }
    /// <summary>
    /// Internal codes of errors
    /// </summary>
    public enum Code
    {
        IncorrectRrequestData = 1,
        UpdateCardNameError,
        CardNotFound,
        CardDeleteError,
        CardAddingError
    }
       
}
