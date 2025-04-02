using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


public class UserExp
{
    [BsonElement("Level")]
    public int Level { get; set; } = 0;

    [BsonElement("Exp")]
    public ulong Exp { get; set; } = 0;

    public bool AddExp(ulong xp)
    {
        Exp += xp;

        if (Exp >= ((int)100 * Math.Pow(Level, 1.7) + 100))
        {
            ++Level;
            return true; // Mean level up
        }

        return false;
    }

    
}