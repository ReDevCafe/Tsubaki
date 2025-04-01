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
            Database.Instance.UpdateMongoUserData(guild.GuildID, message.Author.Id, user);

            if(!asLevelUp) return;

            int level = user.experience.Level;
            ulong fuckingTestREMOVEIFPULLREQUEST = (ulong) Math.Round((100 * Math.Pow(level, 1.8) + 100) - (100 * Math.Pow((level-1), 1.8) + 100));
            var embed = CreateEmbed(
                $"Waw level uuuuuup",
                $"Level {level-1} -> {level} 👑 (next level in {fuckingTestREMOVEIFPULLREQUEST})",
                message.Author,
                Color.Gold
            ).Build();

            await message.Channel.SendMessageAsync(embed: embed);
        }

        public static async Task InteractionMessageHandle(SocketMessage message)
        {
            var handler = new MessageHandle();
            await handler.HandleEventAsync(message);
        }
    }
}