
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Cliente
{    
    [BsonElement("_id")]
    public int Id { get; set; }

    [BsonElement("limite")]
    public int Limite { get; set; }

    [BsonElement("saldo")]
    public int Saldo { get; set; }

    [BsonElement("ultimas_transacoes")]
    public List<Transacao>? Ultimas_transacoes;
}