
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Cliente
{    
    [BsonElement("_id")]
    public int Id { get; set; }

    [BsonElement("limit")]
    public int Limit { get; set; }

    [BsonElement("amount")]
    public int Amount { get; set; }
}