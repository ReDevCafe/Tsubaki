using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Logging 
{
    public class BanHandler 
    {
        public static async Task LogUserBanned(SocketUser user, SocketGuild guild)
        {
            var embed = new EmbedBuilder()
                .WithAuthor(new EmbedAuthorBuilder()
                    .WithName(user.Username)
                    .WithIconUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                    .WithUrl($"https://discordlookup.com/user/{user.Id}")
                )

                .WithDescription($"<@{user.Id}> has been banned")
                .WithUrl($"https://discordlookup.com/user/{user.Id}")
                .WithColor(Color.Purple)
                .WithFooter(
                    new EmbedFooterBuilder()
                    .WithIconUrl(Program.d_client.CurrentUser.GetAvatarUrl())
                    .WithText($"User ID: {user.Id}")
                )

                .WithTimestamp(DateTimeOffset.Now)
                .Build();

            await Program.logChannel.SendMessageAsync(embed: embed);
        }

        public static async Task LogUserUnbanned(SocketUser user, SocketGuild guild)
        {
            var embed = new EmbedBuilder()
                .WithAuthor(new EmbedAuthorBuilder()
                    .WithName(user.Username)
                    .WithIconUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                    .WithUrl($"https://discordlookup.com/user/{user.Id}")
                )

                .WithDescription($"<@{user.Id}> has been unbanned")
                .WithUrl($"https://discordlookup.com/user/{user.Id}")
                .WithColor(new Color(102, 255, 102))
                .WithFooter(
                    new EmbedFooterBuilder()
                    .WithIconUrl(Program.d_client.CurrentUser.GetAvatarUrl())
                    .WithText($"User ID: {user.Id}")
                )

                .WithTimestamp(DateTimeOffset.Now)
                .Build();

            await Program.logChannel.SendMessageAsync(embed: embed);
        }

    }
}