using Maintenance;

namespace Configuration
{
    class Config
    {
        public string Token { get; set; }
        public ulong GuildId { get; set; }
        public ulong LogChannelId { get; set; }
        public int CacheSize { get; set; }

        public LogLevel MinLogLevel { get; set; }
        public bool LogToConsole { get; set; }
    }
}