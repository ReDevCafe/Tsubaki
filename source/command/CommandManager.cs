using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Command.Release;
using Command.Test;
using Discord;
using Discord.WebSocket;
using Maintenance;

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
        }

        private void RegisterCommand(ICommand command)
        {
            commands[command.Name] = command;
            Logger.Instance.Log(LogLevel.Info, $"Command registered: {command.Name}");
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

        public async Task RegisterGuildCommands(SocketGuild guild)
        {
            foreach (var command in commands)
            {
                try
                {
                    await guild.CreateApplicationCommandAsync(command.Value.CommandProperties);
                }
                catch (Exception e)
                {
                    Logger.Instance.Log(LogLevel.Error, $"Failed to create command {command.Value.CommandProperties.Name} in guild {guild.Id}: {e.Message}");
                }
            }

            Logger.Instance.Log(LogLevel.Info, $"Commands registered for guild {guild.Id}");
        }



    }
}
