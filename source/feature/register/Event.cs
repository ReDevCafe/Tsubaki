using Discord.WebSocket;

namespace Register 
{
    public class EventRegistry 
    {
        public static void RegisterEvents(DiscordSocketClient client)
        {
            client.MessageUpdated += Logging.MessageHandler.LogMessageUpdated;
            client.MessageDeleted += Logging.MessageHandler.LogMessageDeleted;

            client.UserBanned += Logging.BanHandler.LogUserBanned;
            client.UserUnbanned += Logging.BanHandler.LogUserUnbanned;

            client.UserUpdated += Logging.UserUpdateHandler.LogUserUpdated;

            client.InviteCreated += Logging.InviteHandler.LogInviteCreated;

            client.UserJoined += Logging.MemberHandler.LogUserJoined;
            client.UserLeft += Logging.MemberHandler.LogUserLeft;
        }
    }
}