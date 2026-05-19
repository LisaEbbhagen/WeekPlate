namespace AiRecipe.Content.Api.Exceptions
{
    public class LlmClientRateLimitException : Exception
    {
        public LlmClientRateLimitException(string message) : base(message)
        {
        }
        public LlmClientRateLimitException(string message, Exception innerException)
        : base(message, innerException) { }
    }
}
