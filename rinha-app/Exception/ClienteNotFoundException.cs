using System.Runtime.Serialization;

[Serializable]
public class ClienteNotFoundException : Exception
{
    public ClienteNotFoundException()
    {
    }

    public ClienteNotFoundException(string? message) : base(message)
    {
    }

    public ClienteNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected ClienteNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}