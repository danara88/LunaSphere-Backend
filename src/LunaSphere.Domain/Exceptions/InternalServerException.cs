namespace LunaSphere.Domain.Exceptions
{
    /// <summary>
    /// Internal server exception
    /// </summary>
    public class InternalServerException : Exception
    {
        public InternalServerException() { }

        public InternalServerException(string message) : base(message) { }
    }
}
