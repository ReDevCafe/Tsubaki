using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Command 
{
    public interface ICommand
    {
        string Name { get; }
        ApplicationCommandProperties CommandProperties { get; }
        Task ExecuteAsync(SocketSlashCommand command);
    }

    public abstract class CommandBase : ICommand
    {
        public abstract string Name { get; }
        public abstract ApplicationCommandProperties CommandProperties { get; }
        public abstract Task ExecuteAsync(SocketSlashCommand command);

    }
}