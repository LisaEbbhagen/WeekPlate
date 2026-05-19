namespace AiRecipe.Content.Api.Exceptions
{
    public class LlmClientUnauthorizedException : Exception
    {
        public  LlmClientUnauthorizedException(string message) : base(message)
        {
        }
        public LlmClientUnauthorizedException(string message, Exception innerException)
        : base(message, innerException) { }
    }
}
