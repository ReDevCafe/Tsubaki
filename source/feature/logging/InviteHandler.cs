using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Logging
{
    public class InviteHandler : EventHandlerBase
    {
        public override async Task HandleEventAsync(params object[] args)
        {
            if (args[0] is SocketInvite invite)
            {
                var embed = new EmbedBuilder()
                    .WithAuthor(new EmbedAuthorBuilder()
                        .WithName(invite.Inviter.Username)
                        .WithIconUrl(invite.Inviter.GetAvatarUrl() ?? invite.Inviter.GetDefaultAvatarUrl())
                        .WithUrl($"https://discordlookup.com/user/{invite.Inviter.Id}")
                    )
                    .WithTitle("Invite Created")
                    .WithDescription($"**Channel:** {invite.Channel.Name}\n" +
                                     $"**Code:** {invite.Code}\n" +
                                     $"**Expires:** {(invite.MaxAge > 0 ? TimeSpan.FromSeconds(invite.MaxAge).ToString() : "Never")}\n" +
                                     $"**Max Uses:** {(invite.MaxUses > 0 ? invite.MaxUses.ToString() : "Unlimited")}")
                    .WithColor(new Color(234, 82, 111))
                    .WithFooter(new EmbedFooterBuilder()
                        .WithIconUrl(Client.CurrentUser.GetAvatarUrl())
                        .WithText($"User ID: {invite.Inviter.Id}")
                    )
                    .WithTimestamp(DateTimeOffset.Now)
                    .Build();

                ulong id = Database.Instance.Guild(invite.Guild.Id).LogChannelId;
                await invite.Guild.GetTextChannel(id).SendMessageAsync(embed: embed);
            }
        }

        public static async Task LogInviteCreated(SocketInvite invite)
        {
            var handler = new InviteHandler();
            await handler.HandleEventAsync(invite);
        }
    }
}