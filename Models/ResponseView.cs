namespace WebIdentityApi.Models
{
#nullable enable
    public class ResponseView<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public ErrorView? Error { get; set; }
    }

    public class ResponseView
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public ErrorView? Error { get; set; }
    }
}