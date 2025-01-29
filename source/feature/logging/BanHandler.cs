using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Logging 
{
    public class BanHandler : EventHandlerBase
    {
        public override async Task HandleEventAsync(params object[] args)
        {
            if (args[0] is SocketUser user && args[1] is SocketGuild guild)
            {
                var embed = CreateEmbed(
                    "User Banned",
                    $"<@{user.Id}> has been banned",
                    user,
                    Color.Purple
                ).Build();

                await LogChannel.SendMessageAsync(embed: embed);
            }
        }

        public static async Task LogUserBanned(SocketUser user, SocketGuild guild)
        {
            var handler = new BanHandler();
            await handler.HandleEventAsync(user, guild);
        }
    }
}