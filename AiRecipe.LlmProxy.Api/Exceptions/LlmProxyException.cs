namespace AiRecipe.LlmProxy.Api.Exceptions
{
    public class LlmProxyException : Exception
    {
        public LlmProxyException(string message) : base(message)
        { 
        }
        public LlmProxyException(string message, Exception innerException)
        : base(message, innerException) { }
    }
}