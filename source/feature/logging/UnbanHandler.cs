using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Logging 
{
    public class UnbanHandler : EventHandlerBase
    {
        public override async Task HandleEventAsync(params object[] args)
        {
            if (args[0] is SocketUser user && args[1] is SocketGuild guild)
            {
                var embed = CreateEmbed(
                    "User unbanned",
                    $"<@{user.Id}> has been unbanned",
                    user,
                    Color.Green
                ).Build();

                await LogChannel.SendMessageAsync(embed: embed);
            }
        }

        public static async Task LogUserUnbanned(SocketUser user, SocketGuild guild)
        {
            var handler = new UnbanHandler();
            await handler.HandleEventAsync(user, guild);
        }
    }
}