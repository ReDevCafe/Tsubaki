using System;
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
            if(!(message is SocketUserMessage userMessage)) return;

            if (message.Channel is not SocketGuildChannel guildChannel) return;
            
            GuildData guild = Database.Instance.Guild(guildChannel.Guild.Id);
            if (guild == null) return; // Probably a good idea to not create a new guild and user;

            UserData user = guild.User(message.Author.Id);
            if (user == null) return;

            bool asLevelUp = user.experience.AddExp((ulong) Math.Round(Math.Log(userMessage.Content.Length + 1) * 10));
            Database.Instance.UpdateMongoUserData(guild.GuildID, message.Author.Id.ToString(), user);

            if(!asLevelUp) return;
            if(guild.LevelChannelId == 0) return;

            int level = user.experience.Level;
            var embed = CreateEmbed(
                $"Waw level uuuuuup",
                $"Level {level-1} -> {level} 👑",
                message.Author,
                Color.Gold
            ).Build();

            await guildChannel.Guild.GetTextChannel(guild.LevelChannelId).SendMessageAsync(embed: embed);
        }

        public static async Task InteractionMessageHandle(SocketMessage message)
        {
            var handler = new MessageHandle();
            await handler.HandleEventAsync(message);
        }
    }
}