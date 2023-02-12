namespace BeCool.Application.Infrastructure
{
    public class JsonResponse
    {
        public bool Error { get; set; }
        public string Message { get; set; }
        public object Value { get; set; }
    }
}
