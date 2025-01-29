using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Logging
{
    public class MemberJoinHandler : EventHandlerBase
    {
        public override async Task HandleEventAsync(params object[] args)
        {
            if (args.Length == 1 && args[0] is SocketGuildUser user)
            {
                if (user == null)
                return;

                var embed = CreateEmbed(
                    $"{user.Username} Joined",
                    $"<@{user.Id}> has joined the guild.\n\n" +
                    $"**Account Created:** <t:{user.CreatedAt.ToUnixTimeSeconds()}:R>\n" +
                    $"**Joined At:** <t:{DateTimeOffset.Now.ToUnixTimeSeconds()}:R>",
                    user,
                    Color.Green,
                    true
                ).Build();

                await LogChannel.SendMessageAsync(embed: embed);
            }
        }

        public static async Task LogMemberJoin(SocketUser user)
        {
            var handler = new MemberJoinHandler();
            await handler.HandleEventAsync(user);
        }
    }

    public class MemberLeftHandler : EventHandlerBase
    {
        public override async Task HandleEventAsync(params object[] args)
        {
            if (args.Length == 2 && args[0] is SocketGuild guild && args[1] is SocketUser user)
            {
                if (user == null || guild == null)
                return;

                var embed = CreateEmbed(
                    $"{user.Username} Left",
                    $"User <@{user.Id}> has left the guild.",
                    user,
                    Color.Red,
                    true
                ).Build();

                await LogChannel.SendMessageAsync(embed: embed);
            }
        }

        public static async Task LogMemberLeft(SocketGuild guild, SocketUser user)
        {
            var handler = new MemberLeftHandler();
            await handler.HandleEventAsync(guild, user);
        }
    }
}
