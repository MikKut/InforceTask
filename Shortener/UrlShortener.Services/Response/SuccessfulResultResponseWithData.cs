namespace UrlShortener.Models.Response
{
    public class SuccessfulResultResponseWithData<T>
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Result { get; set; }
    }
}
