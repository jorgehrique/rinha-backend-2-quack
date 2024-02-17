using System.Runtime.Serialization;

[Serializable]
public class TransacaoInvalidaException : Exception
{
    public TransacaoInvalidaException()
    {
    }

    public TransacaoInvalidaException(string? message) : base(message)
    {
    }

    public TransacaoInvalidaException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected TransacaoInvalidaException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}