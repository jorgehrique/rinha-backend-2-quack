var connection = new Mongo();
var database = connection.getDB("database");

database.createCollection("clientes");

database.clientes.insertMany([
    {
        "id": 1,
        "limit": 100000,
        "amount": 0
    },
    {
      "id": 2,
      "limit": 80000,
      "amount": 0
    },
    {
      "id": 3,
      "limit": 1000000,
      "amount": 0
    },
    {
      "id": 4,
      "limit": 10000000,
      "amount": 0
    },
    {
      "id": 5,
      "limit": 500000,
      "amount": 0
    }
]);