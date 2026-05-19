namespace AiRecipe.LlmProxy.Api.Exceptions
{
    public class LlmTimeOutException : Exception
    {
        public LlmTimeOutException(string message) : base(message)
        {
        }
        public LlmTimeOutException(string message, Exception innerException)
        : base(message, innerException) { }
    }
}
