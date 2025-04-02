using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Command.Test 
{
    public class MongoDebug : CommandBase
    {
        public override string Name => "mongodebug";

        public override ApplicationCommandProperties CommandProperties => 
            new SlashCommandBuilder()
                .WithName("mongodebug")
                .WithDescription("Modify MongoDB")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("target")
                    .WithDescription("Target to add to mongo")
                    .WithType(ApplicationCommandOptionType.String)
                    .AddChoice("Own Guild / Server", "g")
                    .AddChoice("Self", "u")
                    .WithRequired(true)
                )
                .WithDefaultMemberPermissions(GuildPermission.Administrator)
                .Build();

        public override async Task ExecuteAsync(SocketSlashCommand command)
        {
            if(command.GuildId == null) throw new ArgumentNullException("command.GuildId null");
            string choice = command.Data.Options.FirstOrDefault(opt => opt.Name == "target")?.Value?.ToString();
            
            switch(choice)
            {
                case "g":
                    Database.Instance.addGuild((ulong)command.GuildId);
                    await command.RespondAsync($"Added guild {command.GuildId} to the database.");
                    break;

                case "u":
                    if(Database.Instance.Guild((ulong) command.GuildId) == null)
                    {
                        await command.RespondAsync($"This server is not in mongo..");
                        return;
                    }

                    Database.Instance.Guild((ulong) command.GuildId).addUser(command.User.Id);
                    await command.RespondAsync($"Added user {command.User.Username} to guild {command.GuildId}.");
                    break;
            }
        }
    }
}