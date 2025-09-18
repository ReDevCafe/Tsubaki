using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using ZstdSharp.Unsafe;

namespace Command.Release
{
    public class GitLookup : CommandBase
    {
        public override string Name => "git-lookup";

        public override ApplicationCommandProperties CommandProperties =>
            new SlashCommandBuilder()
                .WithName(Name)
                .WithDescription("Search for Class, Struct, Files inside a github organization")
                 .AddOption(new SlashCommandOptionBuilder()
                    .WithName("type")
                    .WithDescription("Type of search")
                    .WithType(ApplicationCommandOptionType.String)
                    .AddChoice("Termes", 0x1)
                    .AddChoice("Commits", 0x2)
                    .WithRequired(true)
                )
                .AddOption("query", ApplicationCommandOptionType.String, "Query", isRequired: true)
                .Build();

        public override Task ExecuteAsync(SocketSlashCommand command)
        {
            string query = command.Data.Options.FirstOrDefault(x => x.Name == "query")?.Value?.ToString();
            byte type = (byte)(command.Data.Options.FirstOrDefault(x => x.Name == "type")?.Value ?? 0);

            switch (type)
            {
                case 0x1:
                    HandleSearchTermes(query);
                    break;
                case 0x2:
                    HandleSearchCommits(query);
                    break;
                default:

                    break;
            }

            return Task.CompletedTask;
        }

        private static void HandleSearchTermes(string query)
        {
            
        }

        private static void HandleSearchCommits(string query)
        {

        }
    }
}