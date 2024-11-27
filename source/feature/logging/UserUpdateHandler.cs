using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Logging
{
    public class UserUpdateHandler
    {
        public static async Task LogUserUpdated(SocketUser before, SocketUser after)
        {
            if (before == null || after == null)
                return;

            if (before.Username != after.Username)
            {
                var embed = new EmbedBuilder()
                    .WithAuthor(new EmbedAuthorBuilder()
                        .WithName($"{after.Username}")
                        .WithIconUrl(after.GetAvatarUrl() ?? after.GetDefaultAvatarUrl())
                        .WithUrl($"https://discordlookup.com/user/{after.Id}")
                    )
                    
                    .WithTitle("Username Changed")
                    .WithDescription($"<@{after.Id}> has updated their username.\n\n**Old Username:** `{before.Username}`\n**New Username:** `{after.Username}`")
                    .WithColor(Color.Magenta)
                    .WithFooter(new EmbedFooterBuilder()
                        .WithIconUrl(Program.d_client.CurrentUser.GetAvatarUrl())
                        .WithText($"User ID: {after.Id}")
                    )

                    .WithTimestamp(DateTimeOffset.Now)
                    .Build();

            
                await Program.logChannel.SendMessageAsync(embed: embed);
            }

            if (before.GetAvatarUrl() != after.GetAvatarUrl())
            {
                var embed = new EmbedBuilder()
                    .WithAuthor(new EmbedAuthorBuilder()
                        .WithName($"{after.Username}")
                        .WithIconUrl(after.GetAvatarUrl() ?? after.GetDefaultAvatarUrl())
                        .WithUrl($"https://discordlookup.com/user/{after.Id}")
                    )

                    .WithTitle("Profile Picture Changed")
                    .WithDescription($"<@{after.Id}> has updated their profile picture.")
                    .WithThumbnailUrl(after.GetAvatarUrl() ?? after.GetDefaultAvatarUrl())
                    .WithColor(Color.Teal)
                    .WithFooter(new EmbedFooterBuilder()
                        .WithIconUrl(Program.d_client.CurrentUser.GetAvatarUrl())
                        .WithText($"User ID: {after.Id}")
                    )

                    .WithTimestamp(DateTimeOffset.Now)
                    .Build();

               
                await Program.logChannel.SendMessageAsync(embed: embed);
            }
        }
    }
}
