namespace prototipo_ia_api.Infra.Utilities
{
    public class GeminiErrorResponse
    {
        public InnerError error { get; set; }
    }
    public class InnerError
    {
        public string status { get; set; }
        public string message { get; set; }
    }
}
