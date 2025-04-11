using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Maintenance;

namespace Command.Test 
{
    public class ExperienceInfo : CommandBase
    {
        public override string Name => "showlevel";

        public override ApplicationCommandProperties CommandProperties => 
            new SlashCommandBuilder()
                .WithName("showlevel")
                .WithDescription("Modify MongoDB")
                .AddOption("user", ApplicationCommandOptionType.User, "The user to look up.", isRequired: true)
                .WithDefaultMemberPermissions(GuildPermission.Administrator)
                .Build();

        public override async Task ExecuteAsync(SocketSlashCommand command)
        {
            if(command.Channel is not SocketGuildChannel guildChannel) return;

            GuildData guild = Database.Instance.Guild(guildChannel.Guild.Id);
            if(guild == null) return;

            SocketUser user = command.User;
            if(user == null) return;

            UserData userData = guild.User(command.User.Id);
            if(userData == null) return;

            int level = userData.experience.Level;
            ulong expToNextLevel = (ulong)(100 * Math.Pow(level, 1.7) + 100);

            var embed = new EmbedBuilder()
                .WithAuthor(new EmbedAuthorBuilder()
                    .WithName(user.Username)
                    .WithIconUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                    .WithUrl($"https://discordlookup.com/user/{user.Id}")
                )
                .WithTitle($"{user.Username}'s Level")
                .WithDescription($"📊 **Level:** {level}\n🌟 **EXP:** {userData.experience.Exp} / {expToNextLevel}")
                .WithColor(Color.Gold)
                .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                .WithFooter(new EmbedFooterBuilder()
                    .WithIconUrl(command.User.GetAvatarUrl())
                    .WithText($"User ID: {user.Id}")
                )
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

            await command.RespondAsync(embed: embed);
        }
    }
}