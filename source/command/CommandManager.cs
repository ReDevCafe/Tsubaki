using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Command.Release;
using Command.Test;
using Discord;
using Discord.WebSocket;

namespace Command
{
    public class CommandManager
    {
        private readonly Dictionary<string, ICommand> commands = new();

        public CommandManager()
        {
            RegisterCommand(new SayCommand());
            RegisterCommand(new UserInfoCommand());

            RegisterCommand(new Mute());
            RegisterCommand(new Kick());
            RegisterCommand(new Ban());
            
            RegisterCommand(new ServerSetup());            
            RegisterCommand(new MongoDebug());
            RegisterCommand(new ExperienceInfo());

            RegisterCommand(new Mute());
            RegisterCommand(new Kick());
            RegisterCommand(new Ban());
        }

        private void RegisterCommand(ICommand command)
        {
            commands[command.Name] = command;
        }

        public async Task HandleCommandAsync(SocketSlashCommand command)
        {
            if (commands.TryGetValue(command.Data.Name, out var handler))
                await handler.ExecuteAsync(command);
            else
                await command.RespondAsync("Unknown command.");
        }

        public IReadOnlyList<ApplicationCommandProperties> GetCommands()
        {
            return commands.Values.Select(c => c.CommandProperties).ToList();
        }

    }
}
