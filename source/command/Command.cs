using System.Threading.Tasks;
using Discord.WebSocket;

namespace Command 
{
    public interface ICommand
    {
        string Name { get; }
        Task ExecuteAsync(SocketSlashCommand command);
    }

    public abstract class CommandBase : ICommand
    {
        public abstract string Name { get; }
        public abstract Task ExecuteAsync(SocketSlashCommand command);
    }
}