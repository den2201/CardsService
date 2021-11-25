namespace CardService.Models
{
    public class ApiResponseModel
    {
        public bool IsOkStatus { get; set; }
        public object Data { get; set; }
        public ErrorMessage ErrorMessage { get; set; }

    }
}
