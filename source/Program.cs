﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Command;
using Configuration;
using Discord;
using Discord.WebSocket;
using Maintenance;
using Newtonsoft.Json;

public class Program 
{
    public static DiscordSocketClient d_client;
    public static DiscordSocketConfig d_config;
    public static Database database;
    public static ConfigFile configuration;
    
    protected static CommandManager t_commandManager = new();

    public static async Task Main()
    {
        configuration = JsonConvert.DeserializeObject<ConfigFile>(File.ReadAllText("config.json"));
        Logger.Instance.Configure("logs.txt", configuration.MinLogLevel, configuration.LogToConsole);

        if (configuration.CacheSize < 0)
        {
            Logger.Instance.Log(LogLevel.Fatal, "Cache size must be > 0");
            return;
        }

        d_config = new DiscordSocketConfig {
            GatewayIntents = GatewayIntents.All, // un peu la flemme mais apres on pourra def les vrai autorisations 
            MessageCacheSize = configuration.CacheSize
        };

        if (string.IsNullOrEmpty(configuration.Token))
        {
            Logger.Instance.Log(LogLevel.Fatal, "Token is empty");
            return;
        }

        d_client = new DiscordSocketClient(d_config);
        
        d_client.SlashCommandExecuted += t_commandManager.HandleCommandAsync;
        d_client.Log += LogAsync;
        d_client.Ready += OnReadyAsync;
        
        database = new(configuration);

        await d_client.LoginAsync(TokenType.Bot, configuration.Token);
        await d_client.StartAsync();
        
        Register.EventRegistry.RegisterEvents(d_client);

        d_client.Disconnected += OnDisconnectedAsync;

        await Task.Delay(-1);
    }

    private static async Task OnReadyAsync()
    {
        await d_client.Rest.DeleteAllGlobalCommandsAsync();

        IReadOnlyList<ApplicationCommandProperties> globalCommands = t_commandManager.GetCommands();
        foreach (ApplicationCommandProperties command in globalCommands)
            await d_client.Rest.CreateGlobalCommand(command);

        Logger.Instance.Log(LogLevel.Info, "Slash command registered");
    }

    private static async Task OnDisconnectedAsync(Exception exception)
    {
        await Task.Delay(500);

        try
        {
            await d_client.StartAsync();
        }
        catch(Exception e)
        {
            Logger.Instance.Log(LogLevel.Fatal, $"FAILED TO RECONNECT, STOPPING. {e.StackTrace}");
            Logger.Instance.Dispose();

            Environment.Exit(e.HResult);
        }

        await Task.CompletedTask;
    }

    private static Task LogAsync(LogMessage log)
    {
        Logger.Instance.Log(LogLevel.Info, log.Message);
        return Task.CompletedTask;
    }
}
