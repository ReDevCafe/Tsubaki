using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Command.Test 
{
    public class SayCommand : CommandBase
    {
        public override string Name => "say";

        public override ApplicationCommandProperties CommandProperties => 
            new SlashCommandBuilder()
                .WithName("say")
                .WithDescription("Repeats the input message.")
                .AddOption("message", ApplicationCommandOptionType.String, "Message to repeat", isRequired: true)
                .WithDefaultMemberPermissions(GuildPermission.ManageMessages)
                .Build();

        public override async Task ExecuteAsync(SocketSlashCommand command)
        {
            var message = command.Data.Options.FirstOrDefault(option => option.Name == "message")?.Value as string;
            await command.RespondAsync(message ?? "You didn't provide a message!");
        }
    }
}