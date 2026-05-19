namespace AiRecipe.LlmProxy.Api.Exceptions
{
    public class LlmUnauthorizedException : Exception
    {
        public LlmUnauthorizedException(string message) : base(message)
        {
        }
        public LlmUnauthorizedException(string message, Exception innerException)
        : base(message, innerException) { }
    }
}
