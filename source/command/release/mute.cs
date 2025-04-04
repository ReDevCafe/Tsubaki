using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Rest;
using Discord;
using System;

namespace Command.Release
{
    public class Mute : CommandBase
    {
        public override string Name => "mute";

        public override ApplicationCommandProperties CommandProperties => 
            new SlashCommandBuilder()
                .WithName(Name)
                .WithDescription("Mute user")
                .AddOption("userid", ApplicationCommandOptionType.User, "User", isRequired: true)
                .AddOption("time", ApplicationCommandOptionType.Integer, "Time in minutes", isRequired: true)
                .AddOption("reason", ApplicationCommandOptionType.String, "Reason")
                .WithDefaultMemberPermissions(GuildPermission.ModerateMembers)
                .Build();

        public override async Task ExecuteAsync(SocketSlashCommand command)
        {
            SocketGuildUser user = (SocketGuildUser) command.Data.Options.First(x => x.Name == "userid").Value;
            int muteDuration     = (int)(long)       command.Data.Options.First(x => x.Name == "time").Value;
            string reason        = command.Data.Options.FirstOrDefault(x => x.Name == "reason")?.Value?.ToString() ?? "No reason provided";
        
            SocketGuild guild = (command.User as SocketGuildUser)?.Guild;
            if(guild == null)
            {
                await command.RespondAsync("This command requires a guild", ephemeral: true);
                return;
            }

            TimeSpan timeoutDuration = TimeSpan.FromMinutes(muteDuration);
            try
            {
                await user.SetTimeOutAsync(timeoutDuration, new RequestOptions { AuditLogReason = reason });
                var embed = new EmbedBuilder()
                    .WithTitle("User Timed Out")
                    .WithColor(Color.Red)
                    .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                    .AddField("User", user.Mention, inline: true)
                    .AddField("Duration", $"{muteDuration} minutes", inline: true)
                    .AddField("Reason", reason, inline: false)
                    .WithFooter($"Muted by {command.User.Username}", (command.User as SocketGuildUser)?.GetAvatarUrl())
                    .WithTimestamp(DateTimeOffset.UtcNow)
                    .Build();

                await command.RespondAsync(embed: embed, ephemeral: true);
                // log in guild log channel
            }
            catch (Exception ex)
            {
                await command.RespondAsync($"Failed to timeout user: {ex.Message}", ephemeral: true);
            }
        }
    }
}