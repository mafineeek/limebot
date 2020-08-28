﻿using LimeBot.Bot.Attributes;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using LimeBot.DAL.Models;
using System.Diagnostics;
using System.Text;
using LimeBot.Bot.Utils;
using LimeBot.DAL;
using DiscordChannel = DSharpPlus.Entities.DiscordChannel;
using DiscordGuild = DSharpPlus.Entities.DiscordGuild;

namespace LimeBot.Bot.Commands
{
    [Category("System")]
    public class SystemCommands : MyCommandModule
    {
        public SystemCommands(GuildContext db) : base(db) { }

        [Command("prefix"), Description("Get or set the prefix"), RequireGuild]
        public async Task GetPrefix(CommandContext ctx)
        {
            await ctx.RespondAsync($"The prefix is `{ctx.Prefix}`");
        }

        [Command("prefix"), RequireUserPermissions(Permissions.ManageMessages), RequireGuild]
        public async Task SetPrefix(CommandContext ctx, string newPrefix)
        {
            guild.Prefix = newPrefix;
            await db.SaveChangesAsync();
            await ctx.RespondAsync($"Set the prefix to `{newPrefix}`");
        }

        [Command("help"), Description("If you are stuck")]
        public async Task Help(CommandContext ctx)
        {
            var url = Config.settings.DashboardUrl;

            var description = new StringBuilder()
                .AppendLine("Hey! I am **LIME**. Nice to see you ;)")
                .AppendLine($"If you are stuck, **[here is a list of my commands]({url}/commands)**.");
            if (ctx.Guild != null) description.AppendLine($"If you want to configure me, **[login to dashboard]({url}/manage/{ctx.Guild.Id})**");
            description.AppendLine($"\n_Protip: type `{ctx.Prefix}help <command>` to get detailed info about specified command_");
              

            var embed = new DiscordEmbedBuilder {
                Title = "Lime help",
                Description = description.ToString(),
                Color = new DiscordColor(Config.settings.embedColor)
            };
            await ctx.RespondAsync(null, false, embed.Build());
        }

        [Command("help")]
        public async Task HelpCommand(CommandContext ctx, [RemainingText] string command)
        {
            var cnext = ctx.Client.GetCommandsNext();
            var cmd = cnext.FindCommand(command, out var _);
            if(cmd == null)
                await ctx.RespondAsync("Command not found!");
            else
                await CommandHelp.SendCommandHelp(ctx, cmd);
        }

        [Command("serverinfo"), Aliases("server", "guild", "guildinfo"), Description("Guild information")]
        public async Task ServerInfo(CommandContext ctx)
        {
            var ds = ctx.Guild;
            var textchannels = ds.Channels.Values.Where(a => isTextChannel(a)).Count();
            var voicechannels = ds.Channels.Values.Where(a => isVoiceChannel(a)).Count();
            var category = ds.Channels.Values.Where(a => a.IsCategory).Count();
            var embed = new DiscordEmbedBuilder
            {
                Title = $"{ds.Name}",
                ThumbnailUrl = ds.IconUrl,
                Color = new DiscordColor(Config.settings.embedColor)
            };
            embed.AddField("**Id:**", $"{ds.Id}", true);
            embed.AddField("**Owner:**", $"{ds.Owner.Mention}", true);
            embed.AddField("**Region:**", $"{RegionUtils.getRegion(ds.VoiceRegion.Name)}", true);
            embed.AddField("**Category**", $"{category}", true);
            embed.AddField("**Text channel**", $"{textchannels}", true);
            embed.AddField("**Voice channel**", $"{voicechannels}", true);
            embed.AddField("**Roles:**", $"{ds.Roles.Count()}", true);
            embed.AddField("**Tier:**", $"{ds.PremiumTier.ToString()}", true);
            embed.AddField("**Verification level:**", $"{ds.VerificationLevel}", true);
            embed.AddField("**Members:**", $"Bots: {ds.Members.Values.Where(a=>a.IsBot).Count()}\nPeoples: {ds.Members.Values.Where(a=>!a.IsBot).Count()}", true);
            StringBuilder builder = new StringBuilder();
            foreach (DiscordEmoji emote in ds.Emojis.Values)
            {
                builder.Append(emote).Append(" ");
            }
            embed.AddField("**Created at:**", $"{ds.CreationTimestamp.DateTime.ToString("dd MMM yyyy")}", true);
            embed.AddField("**Emotes:**", $"{builder}", false);
            await ctx.RespondAsync(embed: embed.Build());
        }

        public bool isTextChannel(DiscordChannel d)
        {
            switch (d.Type)
            {
                case ChannelType.News:
                case ChannelType.Text:
                    return true;
                default:
                    return false;
            }
        }
        
        public bool isVoiceChannel(DiscordChannel d)
        {
            switch (d.Type)
            {
                case ChannelType.Voice:
                    return true;
                default:
                    return false;
            }
        }


        [Command("ping"), Description("LimeBot.Bot ping")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var emoji = DiscordEmoji.FromName(ctx.Client, ":ping_pong:");

            await ctx.RespondAsync($"{emoji} Pong! Ping: {ctx.Client.Ping}ms");
        }

        [Command("userinfo"), Aliases("user"), Description("User information"), RequireGuild]
        public async Task UserInfo(CommandContext ctx, DiscordMember user = null) {
            user ??= ctx.Member;
            var embed = new DiscordEmbedBuilder
            {
                Title = $"{user.Username}#{user.Discriminator}",
                ThumbnailUrl = user.AvatarUrl,
                Color = new DiscordColor(Config.settings.embedColor),
            };

            if (user.Nickname != null) 
                embed.AddField("**Nickname**", user.Nickname, true);

            embed.AddField("**Id:**", ""+user.Id, true);

            embed.AddField("**Account created**", user.CreationTimestamp.DateTime.ToString("dd MMM yyyy"), true);

            embed.AddField("**Joined server**", user.JoinedAt.ToString("dd MMM yyyy"), true);

            embed.AddField("**Top role**", user.Roles.OrderByDescending(i => i.Position).FirstOrDefault()?.Mention ?? "None", true);

            await ctx.RespondAsync(embed: embed.Build());
        }


        [Command("info"), Aliases("botinfo"), Description("LimeBot.Bot information")]
        public async Task BotInfo(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = "Lime LimeBot.Bot info",
                Color = new DiscordColor(Config.settings.embedColor),
                Description = @"Lime is a professional, multi-purpose bot.
Website - [limebot.tk](https://limebot.tk)
Discord server - [Join Lime LimeBot.Bot support](https://discord.gg/9w9EfWh)"
            };
            var users = 0;
            var channels = 0;
            foreach (var g in ctx.Client.Guilds)
            {
                users += g.Value.MemberCount;
                channels += g.Value.Channels.Count;
            }
            embed.AddField("**Stats**",
$@"Guilds: {ctx.Client.Guilds.Count}
Users: {users}
Channels: {channels}");

            using(var proc = Process.GetCurrentProcess())
            {
                var memory = Math.Round(proc.PrivateMemorySize64 / 1e+6, 2);

                var cpu = Math.Round(proc.TotalProcessorTime / (DateTime.Now - proc.StartTime) * 100);
                embed.AddField("**Resource usage**", $"Memory: {memory} MB\nCPU: {cpu}%");
            }

            embed.AddField("**Versions**", 
$@".Net Core: 3.1
Asp.Net Core: 2.2.0
DSharpPlus: {ctx.Client.VersionString}");


            await ctx.RespondAsync(null, false, embed.Build());
        }
    }
}
