using System.Collections.Generic;
using Discord;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class GuildData 
{
    [BsonId]
    public ulong GuildID { get; set; }

    [BsonElement("Users")]
    public Dictionary<ulong, UserData> Users = new();

    public UserData User(ulong userID) => 
        Users.TryGetValue(userID, out UserData user) 
        ? user 
        : null;

    public void addUser(ulong userID)
    {
        UserData user = new UserData();

        Users.Add(userID, user);
        Database.Instance.UpdateMongoUserData(GuildID, userID, user);
    }
}