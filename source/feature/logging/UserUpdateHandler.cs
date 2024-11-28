using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Logging
{
    public class UserUpdateHandler : EventHandlerBase
    {
        public override async Task HandleEventAsync(params object[] args)
        {
            if (args[0] is SocketUser before && args[1] is SocketUser after)
            {
                if (before == null || after == null)
                    return;

                if (before.Username != after.Username)
                {
                    var embed = CreateEmbed(
                        "Username Changed",
                        $"<@{after.Id}> has updated their username.\n\n**Old Username:** `{before.Username}`\n**New Username:** `{after.Username}`",
                        after,
                        Color.Magenta
                    ).Build();
                
                    await Program.logChannel.SendMessageAsync(embed: embed);
                }

                if (before.GetAvatarUrl() != after.GetAvatarUrl())
                {
                    var embed = CreateEmbed(
                        "Profile Picture Changed",
                        $"<@{after.Id}> has updated their profile picture.",
                        after,
                        Color.Teal,
                        true
                    ).Build();
                
                    await Program.logChannel.SendMessageAsync(embed: embed);
                }
            }
        }

        public static async Task LogUserUpdate(SocketUser before, SocketUser after)
        {
            var handler = new UserUpdateHandler();
            await handler.HandleEventAsync(before, after);
        }
    }
}
