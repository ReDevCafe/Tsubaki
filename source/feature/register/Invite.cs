using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Rest;
using Discord.WebSocket;

namespace Register 
{
    public class InviteTracker
    {
        public static readonly Dictionary<ulong, IReadOnlyCollection<RestInviteMetadata>> _inviteCache = new();

        public static async Task InitializeInviteCache(SocketGuild guild)
        {
            if (guild == null)
                return;

            var invites = await guild.GetInvitesAsync();
            _inviteCache[guild.Id] = invites;
        }
    }
}