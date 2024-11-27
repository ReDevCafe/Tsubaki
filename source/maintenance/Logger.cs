using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Unicode;

namespace Maintenance 
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error,
        Fatal,
        Debug,
    }

    public class Logger
    {
        private static readonly Lazy<Logger> instance = new (() => new Logger());
        private readonly object _lock = new();
        private StreamWriter writer;

        public LogLevel minimumLevel { get; set; } = LogLevel.Info;
        public bool logToConsole { get; set; } = true;

        private Logger() {}

        public static Logger Instance => instance.Value;

        public void Configure(string logPath = "test.log", LogLevel minLogLevel = LogLevel.Info, bool logToConsole = true)
        {
            minimumLevel = minLogLevel;
            this.logToConsole = logToConsole;

            if(string.IsNullOrEmpty(logPath))
            {
                Console.WriteLine("Invalid log path: " + logPath);
                return;
            }

            lock (_lock)
            {
                writer?.Dispose();
                writer = new StreamWriter(logPath, append: true, encoding: Encoding.UTF8)
                {
                    AutoFlush = true
                };
            }
        }

        public void Log(LogLevel level, string message)
        {
            if (level < minimumLevel)
                return;

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string logMessage = $"[{timestamp}] [{level}] {message}\n";

            lock (_lock)
            {
                writer?.Write(logMessage);

                if(!logToConsole)
                    return;

                logMessage = $"\x1b[97m[{timestamp}] {GetLogLevelColor(level)}[{level}] \x1b[39m{message}";
                Console.WriteLine(logMessage);
            }
        }

        public void Dispose()
        {
            lock (_lock)
                writer?.Dispose();
        }

        private string GetLogLevelColor(LogLevel level)
        {
            return level switch
            {
                LogLevel.Warning => "\x1b[93m",
                LogLevel.Error =>   "\x1b[91m",
                LogLevel.Debug =>   "\x1b[96m",
                _ => "\x1b[39m"
            };
        }
    }
}