using System.Collections.Generic;
using Discord;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

public class GuildData 
{
    [BsonId]
    public ulong GuildID { get; set; }

    [BsonElement("LogChannelId")]
    public ulong LogChannelId { get; set; } = 0;

    [BsonElement("LevelChannelId")]
    public ulong LevelChannelId { get; set; } = 0;

    [BsonElement("Users")]
    [BsonDictionaryOptions(DictionaryRepresentation.Document)]
    public Dictionary<string, UserData> Users = new();


    public UserData User(ulong userID) => 
        Users.GetValueOrDefault(userID.ToString());

    public void addUser(ulong userID)
    {
        string userIdStr = userID.ToString(); // Convert ulong to string
        UserData user = new UserData();
        Users[userIdStr] = user;
        Database.Instance.UpdateMongoUserData(GuildID, userIdStr, user);
    }
}