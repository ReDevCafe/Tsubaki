using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Configuration;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;

public class Program 
{
    public static DiscordSocketClient d_client;
    public static DiscordSocketConfig d_config;
    public static Config configuration;
    public static ITextChannel logChannel;

    public static async Task Main()
    {
        // Load configuration
        configuration = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));

        if (configuration.CacheSize < 0)
        {
            Console.WriteLine("No cache size found in config.json");
            return;
        }

        d_config = new DiscordSocketConfig {
            GatewayIntents = GatewayIntents.All, // un peu la flemme mais apres on pourra def les vrai autorisations 
            MessageCacheSize = configuration.CacheSize
        };

        if (string.IsNullOrEmpty(configuration.Token))
        {
            Console.WriteLine("No token found in config.json");
            return;
        }

        d_client = new DiscordSocketClient(d_config);
        
        d_client.Log += LogAsync;
        d_client.Ready += OnReadyAsync;
        d_client.SlashCommandExecuted += Commands.SlashCommandHandler;

        await d_client.LoginAsync(TokenType.Bot, configuration.Token);
        await d_client.StartAsync();

        logChannel = await d_client.GetChannelAsync(configuration.LogChannelId) as ITextChannel;
        Register.EventRegistry.RegisterEvents(d_client);

        await Task.Delay(-1);
    }

    private static async Task OnReadyAsync()
    {
        ApplicationCommandProperties[] commands = Commands.commands.ToArray();
        if(commands.Length == 0 || commands == null) 
        {
            await LogAsync(new LogMessage(LogSeverity.Warning, "Startup", "No slash commands found."));
            return;
        }

        await d_client.BulkOverwriteGlobalApplicationCommandsAsync(commands);
        await LogAsync(new LogMessage(LogSeverity.Info, "Startup", "Slash commands registered."));
    }

    private static Task LogAsync(LogMessage log)
    {
        Console.WriteLine(log);
        return Task.CompletedTask;
    }
}
