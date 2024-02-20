using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

public class Transacao
{
    [JsonIgnore]
    [BsonId(IdGenerator = typeof(ObjectIdGenerator))]    
    public ObjectId? Id { get; set; }

    [JsonIgnore]
    [BsonElement("cliente_id")]
    public int? ClienteId { get; set; }

    [BsonElement("valor")]
    public int Valor { get; set; }

    [BsonElement("tipo")]
    public char Tipo { get; set; }

    [BsonElement("descricao")]
    public string Descricao { get; set; }

    [BsonElement("realizada_em")]
    public string? Realizada_em { get; set; }

    public Transacao(int ClienteId, int? Valor, char? Tipo, string? Descricao)
    {
        this.ClienteId = ClienteId;
        this.Valor = Valor ?? 0;
        this.Tipo = Tipo ?? 'c';
        this.Descricao = Descricao ?? "";
        Realizada_em = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK");
    }

}
