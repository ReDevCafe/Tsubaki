using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Logging 
{
    public class MessageUpdateHandler : EventHandlerBase
    {
        public override async Task HandleEventAsync(params object[] args)
        {
            if(args[0] is Cacheable<IMessage, ulong> before && args[1] is SocketMessage after && args[2] is ISocketMessageChannel channel)
            {
                if (!before.HasValue)
                    await before.GetOrDownloadAsync();
                
                if (after is not IUserMessage newMessage || newMessage.Channel == null || newMessage.Author.IsBot)
                    return;

                var embed = CreateEmbed(
                    "Message has been edited in the channel: " + channel.Name,
                    $"```diff\n- {before.Value.Content}\n+ {newMessage.Content}\n```",
                    newMessage.Author,
                    Color.Orange
                ).WithUrl(newMessage.GetJumpUrl()).Build();

                await LogChannel.SendMessageAsync(embed: embed);
            }
        }

        public static async Task LogMessageUpdate(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            var handler = new MessageUpdateHandler();
            await handler.HandleEventAsync(before, after, channel);
        }
        
    }
}