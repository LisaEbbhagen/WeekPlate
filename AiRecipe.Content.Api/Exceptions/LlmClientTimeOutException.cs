namespace AiRecipe.Content.Api.Exceptions
{
    public class LlmClientTimeOutException : Exception
    {
        public LlmClientTimeOutException(string message) : base(message)
        { 
        }
        public LlmClientTimeOutException(string message, Exception innerException)
        : base(message, innerException) { }
    }
}