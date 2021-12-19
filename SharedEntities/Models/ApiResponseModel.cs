namespace SharedEntities.Models
{
    /// <summary>
    /// response model.
    /// </summary>
    /// <remarks>
    /// if Action returns OK Status flag is true else flag is false. 
    /// And there is may be some data in the Data property.
    /// If Action returns Error Status. Data property is empty and ErrorMessage property contains info about error
    /// </remarks>
    public class ApiResponseModel<T>
    {
        public bool IsOkStatus { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
