using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Transacao
{
    [BsonElement("valor")]
    public int Valor { get; set; }

    [BsonElement("tipo")]
    public char Tipo { get; set; }

    [BsonElement("descricao")]
    public string Descricao { get; set; }

    [BsonElement("realizada_em")]
    public string? Realizada_em { get; set; }

    public Transacao(TransacaoBody body)
    {
        Valor = body.valor ?? 0;
        Tipo = body.tipo ?? 'c';
        Descricao = body.descricao ?? "";
        Realizada_em = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK");
    }

}
