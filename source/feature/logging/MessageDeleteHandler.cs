using System.Threading.Tasks;
using Discord;

namespace Logging 
{
    public class MessageDeleteHandler : EventHandlerBase
    {
        public override async Task HandleEventAsync(params object[] args)
        {
            if(args[0] is Cacheable<IMessage, ulong> cacheableMessage && args[1] is Cacheable<IMessageChannel, ulong> deletedMessage)
            {
                if (!cacheableMessage.HasValue)
                    await cacheableMessage.GetOrDownloadAsync();
                

                var message = cacheableMessage.Value;
                var embed = CreateEmbed(
                    "Message has been deleted in the channel: " + message.Channel.Name,
                    $"```diff\n- {message.Content}\n```",
                    message.Author,
                    Color.Red
                ).WithUrl(message.GetJumpUrl()).Build();

                await LogChannel.SendMessageAsync(embed: embed);
            }
        }

        public static async Task LogMessageDelete(Cacheable<IMessage, ulong> cacheableMessage, Cacheable<IMessageChannel, ulong> deletedMessage)
        {
            var handler = new MessageDeleteHandler();
            await handler.HandleEventAsync(cacheableMessage, deletedMessage);
        }
    }
}