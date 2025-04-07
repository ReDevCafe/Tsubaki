using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Configuration;
using Maintenance;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

public class Database 
{
    MongoClient mongoClient;
    IMongoDatabase mongoDatabase;
    IMongoCollection<GuildData> mongoCollection;

    public static Database Instance;

    public Database(ConfigFile config)
    {
        if(config.MongoHost == null || config.MongoDatabase == null || config.MongoCollection == null)
        {
            Logger.Instance.Log(LogLevel.Fatal, "Database configuration is fucked up.");
            throw new Exception("Invalid database configuration");
        }

        mongoClient = new MongoClient(config.MongoHost);
        mongoDatabase = mongoClient.GetDatabase(config.MongoDatabase);
        mongoCollection = mongoDatabase.GetCollection<GuildData>(config.MongoCollection);

        Instance = this;
    }

    public GuildData Guild(ulong guildId)
    {
        GuildData guildData = mongoCollection.Find(g => g.GuildID == guildId).FirstOrDefault();
        return guildData;
    }

    public GuildData addGuild(ulong guildId)
    {
        GuildData guildData = Guild(guildId);
        if(guildData != null) return guildData;

        try
        {
            guildData = new GuildData { GuildID = guildId };
            mongoCollection.InsertOne(guildData);
            return guildData;
        }
        catch(Exception ex)
        {
            Logger.Instance.Log(LogLevel.Fatal, $"Error adding guild to database: {ex.Message}");
            guildData = null;
        }

        return guildData;
    }

    public void UpdateMongoUserData(ulong guildId, string userId, UserData data)
    {
        var filter = Builders<GuildData>.Filter.Eq("_id", guildId);

        if (filter == null) throw new Exception();

        var update = Builders<GuildData>.Update
            .Set($"Users.{userId}", data);

        mongoCollection.UpdateOne(filter, update);
    }



    public void UpdateMongoGuildData(ulong guildId, GuildData data)
    {
        try
        {
            var filter = Builders<GuildData>.Filter.Eq("_id", guildId);
            mongoCollection.ReplaceOne(filter, data);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            throw;
        }
        
    }
}