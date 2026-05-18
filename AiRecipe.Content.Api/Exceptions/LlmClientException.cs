namespace AiRecipe.Content.Api.Exceptions
{
    public class LlmClientException : Exception
    {
        public LlmClientException(string message) : base(message)
        { 
        }
        public LlmClientException(string message, Exception innerException)
        : base(message, innerException) { }
    }
}