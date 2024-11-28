using Discord.WebSocket;
using Maintenance;

namespace Register 
{
    public class EventRegistry 
    {
        public static void RegisterEvents(DiscordSocketClient client)
        {
            client.MessageUpdated += Logging.MessageUpdateHandler.LogMessageUpdate;
            client.MessageDeleted += Logging.MessageDeleteHandler.LogMessageDelete;

            client.UserBanned += Logging.BanHandler.LogUserBanned;
            client.UserUnbanned += Logging.UnbanHandler.LogUserUnbanned;

            client.UserUpdated += Logging.UserUpdateHandler.LogUserUpdate;

            client.InviteCreated += Logging.InviteHandler.LogInviteCreated;

            client.UserJoined += Logging.MemberJoinHandler.LogMemberJoin;
            client.UserLeft += Logging.MemberLeftHandler.LogMemberLeft;

            Logger.Instance.Log(LogLevel.Debug, "Events registered");
        }
    }
}