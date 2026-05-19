namespace AiRecipe.LlmProxy.Api.Exceptions
{
    public class LlmRateLimitException : Exception
    {
        public LlmRateLimitException(string message) : base(message)
        {
        }
        public LlmRateLimitException(string message, Exception innerException)
        : base(message, innerException) { }
    }
}
