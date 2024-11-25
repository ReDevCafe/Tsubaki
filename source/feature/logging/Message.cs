using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Logging 
{
    public class MessageHandler
    {
        public static async Task LogMessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            if (channel == null)
                return;

            if (!before.HasValue || before.Value == null)
            {
                await before.GetOrDownloadAsync();

                return;
            }

            if (after is not IUserMessage newMessage || newMessage.Channel == null || newMessage.Author.IsBot)
                return;


            var user = newMessage.Author;
            var embed = new EmbedBuilder()
                .WithAuthor(new EmbedAuthorBuilder()
                    .WithName(user.Username)
                    .WithIconUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                    .WithUrl($"https://discordlookup.com/user/{user.Id}")
                )

                .WithTitle($"Message has been edited in the channel: {newMessage.Channel.Name}")
                .WithUrl(newMessage.GetJumpUrl())
                .WithDescription($"```diff\n- {before.Value.Content}"+
                                 $"\n+ {newMessage.Content}\n```")
                .WithColor(Color.Orange)
                .WithFooter(
                    new EmbedFooterBuilder()
                    .WithIconUrl(Program.d_client.CurrentUser.GetAvatarUrl())
                    .WithText($"User ID: {user.Id}")
                )

                .WithTimestamp(DateTimeOffset.Now)
                .Build();

            await Program.logChannel.SendMessageAsync(embed: embed);
        }

        public static async Task LogMessageDeleted(Cacheable<IMessage, ulong> cacheableMessage, Cacheable<IMessageChannel, ulong> deletedMessage)
        {
            if (!cacheableMessage.HasValue)
               await cacheableMessage.GetOrDownloadAsync();

            if (!cacheableMessage.HasValue || cacheableMessage.Value == null)
                return;

            var message = cacheableMessage.Value;
            var user = message.Author;
            var embed = new EmbedBuilder()
                .WithAuthor(new EmbedAuthorBuilder()
                    .WithName(user.Username)
                    .WithIconUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                    .WithUrl($"https://discordlookup.com/user/{user.Id}")
                )
                
                .WithTitle($"Message has been deleted in the channel: {message.Channel.Name}")
                .WithUrl(message.GetJumpUrl())
                .WithDescription($"**Message:** ```diff\n- {message.Content}\n```")
                .WithColor(Color.Red)
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