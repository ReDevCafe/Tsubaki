using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Command.Release
{
    public class Ban : CommandBase
    {
        public override string Name => "ban";

        public override ApplicationCommandProperties CommandProperties =>
            new SlashCommandBuilder()
                .WithName(Name)
                .WithDescription("Ban a user from the server")
                .AddOption("userid", ApplicationCommandOptionType.User, "User", isRequired: true)
                .AddOption("reason", ApplicationCommandOptionType.String, "Reason")
                .WithDefaultMemberPermissions(GuildPermission.BanMembers)
                .Build();

        public override async Task ExecuteAsync(SocketSlashCommand command)
        {
            SocketGuildUser user = (SocketGuildUser)command.Data.Options.First(x => x.Name == "userid").Value;
            string reason = command.Data.Options.FirstOrDefault(x => x.Name == "reason")?.Value?.ToString() ?? "No reason provided";

            SocketGuild guild = (command.User as SocketGuildUser)?.Guild;
            if (guild == null)
            {
                await command.RespondAsync("This command requires a guild", ephemeral: true);
                return;
            }

            if (!guild.CurrentUser.GuildPermissions.BanMembers)
            {
                await command.RespondAsync("I do not have permission to ban members.", ephemeral: true);
                return;
            }

            try
            {
                await guild.AddBanAsync(user, 0, reason);

                var embed = new EmbedBuilder()
                    .WithTitle("User Banned")
                    .WithColor(Color.DarkRed)
                    .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                    .AddField("User", user.Mention, inline: true)
                    .AddField("Reason", reason, inline: false)
                    .WithFooter($"Banned by {command.User.Username}", (command.User as SocketGuildUser)?.GetAvatarUrl())
                    .WithTimestamp(DateTimeOffset.UtcNow)
                    .Build();

                await command.RespondAsync(embed: embed, ephemeral: true);
                // log in guild log channel
            }
            catch (Exception ex)
            {
                await command.RespondAsync($"Failed to ban user: {ex.Message}", ephemeral: true);
            }
        }
    }
}