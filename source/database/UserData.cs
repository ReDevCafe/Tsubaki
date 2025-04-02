using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class UserData 
{
    [BsonElement("Experience system")]
    public UserExp experience {  get; set; } = new();

    public UserData() {}
}