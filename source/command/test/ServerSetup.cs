using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Command.Test 
{
    public class ServerSetup : CommandBase
    {
        public override string Name => "serversetup";

        public override ApplicationCommandProperties CommandProperties => 
            new SlashCommandBuilder()
                .WithName("serversetup")
                .WithDescription("Set up a guild")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("target")
                    .WithDescription("Target to set")
                    .WithType(ApplicationCommandOptionType.String)
                    .AddChoice("Log Channel", "loc")
                    .AddChoice("Level Channel", "lec")
                    .WithRequired(true)
                )
                .AddOption(
                    new SlashCommandOptionBuilder()
                   .WithName("channel")
                   .WithDescription("value")
                   .WithType(ApplicationCommandOptionType.Channel)
                )
                .WithDefaultMemberPermissions(GuildPermission.Administrator)
                .Build();

        public override async Task ExecuteAsync(SocketSlashCommand command)
        {
            if(command.GuildId == null) throw new ArgumentNullException("command.GuildId null");
            string choice = command.Data.Options.FirstOrDefault(opt => opt.Name == "target")?.Value?.ToString();
            SocketTextChannel channel = command.Data.Options.FirstOrDefault(opt => opt.Name == "channel")?.Value as SocketTextChannel;

            GuildData guild = Database.Instance.Guild((ulong)command.GuildId);
            if(guild == null) guild = Database.Instance.addGuild((ulong)command.GuildId);

            switch(choice)
            {
                case "loc":
                    if(channel == null)
                    {
                        await command.RespondAsync($"Please provide a valid channel.");
                        return;
                    }

                    guild.LogChannelId = channel.Id;
                    break;

                case "lec":
                    if(channel == null) 
                    {
                        await command.RespondAsync($"Please provide a valid channel.");
                        return;
                    }

                    guild.LevelChannelId = channel.Id;
                    break;
            }

            Database.Instance.UpdateMongoGuildData((ulong)command.GuildId, guild);
            await command.RespondAsync($"Guild setup updated.", ephemeral: true);
        }
    }
}