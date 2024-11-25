using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace Logging
{
    public class MemberHandler
    {
        public static async Task LogUserJoined(SocketGuildUser user)
        {
             if (user == null)
                return;

            var embed = new EmbedBuilder()
                .WithAuthor(new EmbedAuthorBuilder()
                    .WithName($"{user.Username}")
                    .WithIconUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                    .WithUrl($"https://discordlookup.com/user/{user.Id}")
                )
                .WithTitle($"{user.Username} Joined")
                .WithDescription($"<@{user.Id}> has joined the guild.\n\n" +
                                 $"**Account Created:** <t:{user.CreatedAt.ToUnixTimeSeconds()}:R>\n" +
                                 $"**Joined At:** <t:{DateTimeOffset.Now.ToUnixTimeSeconds()}:R>")
                .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                .WithColor(Color.Green)
                .WithFooter(new EmbedFooterBuilder()
                    .WithIconUrl(Program.d_client.CurrentUser.GetAvatarUrl())
                    .WithText($"User ID: {user.Id}")
                )
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

            await Program.logChannel.SendMessageAsync(embed: embed);
        }

        public static async Task LogUserLeft(SocketGuild guild, SocketUser user)
        {
            if (user == null || guild == null)
                return;

            var embed = new EmbedBuilder()
                .WithAuthor(new EmbedAuthorBuilder()
                    .WithName($"{user.Username}")
                    .WithIconUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                    .WithUrl($"https://discordlookup.com/user/{user.Id}")
                )
                .WithTitle($"{user.Username} left")
                .WithDescription($"User <@{user.Id}> has left the guild.")
                .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                .WithColor(Color.Red)
                .WithFooter(new EmbedFooterBuilder()
                    .WithIconUrl(Program.d_client.CurrentUser.GetAvatarUrl())
                    .WithText($"User ID: {user.Id}")
                )
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

            await Program.logChannel.SendMessageAsync(embed: embed);
        }
    }
}
