namespace CardService.Models
{
    public class ApiResponseModel
    {
        bool IsOkStatus { get; set; }
        object Data { get; set; }
        ErrorMessage ErrorMessage { get; set; }

    }
}
