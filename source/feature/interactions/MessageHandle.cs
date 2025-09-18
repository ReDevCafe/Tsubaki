using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Logging;

namespace Interaction 
{
    public class MessageHandle : EventHandlerBase
    {
        public override async Task HandleEventAsync(params object[] args)
        {
            if (args.Length == 0 || args[0] is not SocketMessage message) return;
            if (!(message is SocketUserMessage userMessage)) return;

            if (message.Channel is not SocketGuildChannel guildChannel) return;

            GuildData guild = Database.Instance.Guild(guildChannel.Guild.Id);
            if (guild == null) return; // Probably a good idea to not create a new guild and user;

            UserData user = guild.User(message.Author.Id);
            if (user == null) return;

            string messageLower = userMessage.Content;
            var word = Regex.Matches(messageLower, @"\w+").Cast<Match>().Select(m => m.Value).ToList();

            double score = 1;
            if (Regex.Matches(messageLower, @"(.)\1{3,}", RegexOptions.IgnoreCase).Count == 0)
            {
                score += Math.Min(messageLower.Length, 700) / 700.0 * 400;
                score += word.Distinct().Count() / (double)word.Count() * 300;
            }

            if (word.Contains("merci") || word.Contains(":3") ||
                word.Contains("bonjour") || word.Contains("salut") ||
                word.Contains("coucou")
            )
                score += 150;

            if (word.Contains("!") || word.Contains("?"))
                score += 25;

            bool asLevelUp = user.experience.AddExp((ulong)score);
            Database.Instance.UpdateMongoUserData(guild.GuildID, message.Author.Id.ToString(), user);

            if(!asLevelUp) return;
            if(guild.LevelChannelId == 0) return;

            int level = user.experience.Level;
            var embedLevel = CreateEmbed(
                $"Waw level uuuuuup",
                $"Level {level-1} -> {level} 👑",
                message.Author,
                Color.Gold
            ).Build();

            await guildChannel.Guild.GetTextChannel(guild.LevelChannelId).SendMessageAsync(embed: embedLevel);
        }

        public static async Task InteractionMessageHandle(SocketMessage message)
        {
            var handler = new MessageHandle();
            await handler.HandleEventAsync(message);
        }
    }
}