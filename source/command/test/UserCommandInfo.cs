using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Rest;
using Discord;

namespace Command.Test
{
    public class UserInfoCommand : CommandBase
    {
        public override string Name => "userinfo";

        public override ApplicationCommandProperties CommandProperties => 
            new SlashCommandBuilder()
                .WithName("userinfo")
                .WithDescription("Get user information")
                .AddOption("userid", ApplicationCommandOptionType.String, "The ID of the user to look up.", isRequired: true)
                .Build();

        public override async Task ExecuteAsync(SocketSlashCommand command)
        {
            var userIdArg = command.Data.Options.FirstOrDefault(option => option.Name == "userid")?.Value as string;
            if (string.IsNullOrEmpty(userIdArg) || !ulong.TryParse(userIdArg, out var userId))
            {
                await command.RespondAsync("Invalid user ID provided!");
                return;
            }

            IUser user = await Program.d_client?.Rest.GetUserAsync(userId);
            if (user == null)
            {
                await command.RespondAsync("User not found!");
                return;
            }

            var embed = new EmbedBuilder()
                .WithTitle("User Information")
                .WithDescription($"<@{user.Id}> information retrieved from Discord.")
                .AddField("Global Name", $"{user.GlobalName} *({user.Username})*")
                .AddField("ID", user.Id)
                .AddField("Created At", user.CreatedAt.ToString("g"))
                .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                .WithFooter(
                    new EmbedFooterBuilder()
                    .WithIconUrl(Program.d_client.CurrentUser.GetAvatarUrl())
                    .WithText($"Requester ID: {command.User.Id}")
                )
                .WithColor(Color.DarkTeal)
                .Build();

            await command.RespondAsync(embed: embed);
        }
    }
}
