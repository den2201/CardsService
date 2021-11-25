namespace CardService.Models
{
    /// <summary>
    /// response model.
    /// </summary>
    /// <remarks>
    /// if Action returns OK Status flag is true else flag is false. 
    /// And there is may be some data in the Data property.
    /// If Action returns Error Status. Data property is empty and ErrorMessage property contains info about error
    /// </remarks>
    public class ApiResponseModel
    {
        public bool IsOkStatus { get; set; }
        public object Data { get; set; }
        public ErrorMessage ErrorMessage { get; set; }

    }
}
