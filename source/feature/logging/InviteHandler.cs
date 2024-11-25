using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Logging
{
    public class InviteHandler
    {
        public static async Task LogInviteCreated(SocketInvite invite)
        {
            if (invite == null)
                return;

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
                .WithFooter(
                    new EmbedFooterBuilder()
                    .WithIconUrl(Program.d_client.CurrentUser.GetAvatarUrl())
                    .WithText($"User ID: {invite.Inviter.Id}")
                )
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

            
            await Program.logChannel.SendMessageAsync(embed: embed);
        }
    }
}
