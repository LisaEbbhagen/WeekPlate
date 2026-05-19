namespace AiRecipe.LlmProxy.Api.Exceptions
{
    public class LlmForbiddenException : Exception
    {
        public LlmForbiddenException(string message) : base(message)
        {
        }
        public LlmForbiddenException(string message, Exception innerException)
        : base(message, innerException) { }
    }
}
