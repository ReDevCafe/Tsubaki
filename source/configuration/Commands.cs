using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Configuration
{
    public class Commands 
    {
        public static readonly List<ApplicationCommandProperties> commands = new()
        {
            new SlashCommandBuilder()
                .WithName("say")
                .WithDescription("Repeats the input message.")
                .AddOption("message", ApplicationCommandOptionType.String, "Message to repeat", isRequired: true)
                .Build(),

            new SlashCommandBuilder()
                .WithName("add")
                .WithDescription("Adds two numbers.")
                .AddOption("num1", ApplicationCommandOptionType.Integer, "First number", isRequired: true)
                .AddOption("num2", ApplicationCommandOptionType.Integer, "Second number", isRequired: true)
                .Build()
        };

        public static async Task SlashCommandHandler(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "say":
                    var message = command.Data.Options.FirstOrDefault(option => option.Name == "message")?.Value as string;
                    await command.RespondAsync(message ?? "You didn't provide a message!");
                    break;

                case "add":
                    var num1 = (long)command.Data.Options.FirstOrDefault(option => option.Name == "num1")?.Value;
                    var num2 = (long)command.Data.Options.FirstOrDefault(option => option.Name == "num2")?.Value;
                    await command.RespondAsync($"The sum of {num1} and {num2} is {num1 + num2}.");
                    break;

                default:
                    await command.RespondAsync("Unknown command.");
                    break;
            }
        }
    }


}