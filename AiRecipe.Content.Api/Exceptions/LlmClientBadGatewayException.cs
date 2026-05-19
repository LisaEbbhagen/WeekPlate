namespace AiRecipe.Content.Api.Exceptions
{
    public class LlmClientBadGatewayException : Exception
    {
        public LlmClientBadGatewayException(string message) : base(message)
        { 
        }
        public LlmClientBadGatewayException(string message, Exception innerException)
        : base(message, innerException) { }
    }
}