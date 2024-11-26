using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace Command.Test 
{
    public class SayCommand : CommandBase
    {
        public override string Name => "say";

        public override async Task ExecuteAsync(SocketSlashCommand command)
        {
            var message = command.Data.Options.FirstOrDefault(option => option.Name == "message")?.Value as string;
            await command.RespondAsync(message ?? "You didn't provide a message!");
        }
    }
}