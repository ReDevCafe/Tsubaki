using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Logging
{
    public abstract class EventHandlerBase
    {
        protected static DiscordSocketClient Client => Program.d_client;
        protected static ITextChannel LogChannel => Program.logChannel;

        public abstract Task HandleEventAsync(params object[] args);

        protected EmbedBuilder CreateEmbed(string title, string description, IUser user, Color color, bool showPicture = false)
        {
            return new EmbedBuilder()
                .WithAuthor(new EmbedAuthorBuilder()
                    .WithName(user.Username)
                    .WithIconUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                    .WithUrl($"https://discordlookup.com/user/{user.Id}")
                )
                .WithTitle(title)
                .WithDescription(description)
                .WithColor(color)
                .WithThumbnailUrl(showPicture ? user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl() : null)
                .WithFooter(new EmbedFooterBuilder()
                    .WithIconUrl(Client.CurrentUser.GetAvatarUrl())
                    .WithText($"User ID: {user.Id}")
                )
                .WithTimestamp(DateTimeOffset.Now);
        }
    }
}
