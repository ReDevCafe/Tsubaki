using Discord.WebSocket;
using Maintenance;
using Interaction;

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

            client.MessageReceived += Interaction.MessageHandle.InteractionMessageHandle;

            Logger.Instance.Log(LogLevel.Debug, "Events registered");
        }
    }
}