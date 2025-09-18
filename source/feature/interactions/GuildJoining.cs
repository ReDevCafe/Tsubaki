using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Logging;

namespace Interaction 
{
    public class GuildJoining : EventHandlerBase
    {
        #pragma warning disable CS1998
        public override async Task HandleEventAsync(params object[] args)
        {
            if (args.Length == 0 || args[0] is not SocketGuild socketGuild) return;

            Database.Instance.addGuild(socketGuild.Id);
            await Program.t_commandManager.RegisterGuildCommands(socketGuild);
        }

        public static async Task InteractionGuildJoiningHandle(SocketGuild socketGuild)
        {
            var handler = new GuildJoining();
            await handler.HandleEventAsync(socketGuild);
        }
    }
}