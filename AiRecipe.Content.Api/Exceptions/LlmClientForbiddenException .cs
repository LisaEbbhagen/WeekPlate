namespace AiRecipe.Content.Api.Exceptions
{
    public class LlmClientForbiddenException : Exception
    {
        public LlmClientForbiddenException(string message) : base(message)
        {
        }
        public LlmClientForbiddenException(string message, Exception innerException)
        : base(message, innerException) { }
    }
}
