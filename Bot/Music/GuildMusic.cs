﻿using DAL;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.Lavalink.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Music
{
    public class GuildMusic
    {
        private Lavalink lava;
        private DiscordGuild guild;
        private DiscordChannel textChannel;
        public LavalinkGuildConnection player;
        public List<LavalinkTrack> Queue { get; private set; } = new List<LavalinkTrack>();
        public int Index { get; private set; } = 0;
        private bool skipped = false;

        public bool isPaused=false;

        public GuildMusic(Lavalink lava, DiscordGuild guild, DiscordChannel textChannel)
        {
            this.lava = lava;
            this.guild = guild;
            this.textChannel = textChannel;
        }

        public async Task InitPlayer(DiscordChannel vc)
        {
            player = await lava.node.ConnectAsync(vc);
            player.PlaybackFinished += PlaybackFinished;
        }

        private async Task PlaybackFinished(TrackFinishEventArgs e)
        {
            if (Index < Queue.Count - 1) await textChannel.SendMessageAsync("Queue ended");
            else if (skipped) skipped = false;
            else await Next();
        }

        public async Task Add(LavalinkTrack track)
        {
            Queue.Add(track);
            if (Queue.Count == 1)
            {
                await Play();
            }
        }

        private async Task Play()
        {
            await player.PlayAsync(Queue[Index]);
        }

        public async Task Next()
        {
            if (Index < Queue.Count - 1)
            {
                Index++;
                skipped = true;
                await Play();
                var embed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor(Config.settings.embedColor),
                    Title = $"Now playing: **[{Queue[Index].Title}]({Queue[Index].Uri})**"
                };
                await textChannel.SendMessageAsync(embed: embed);
            }
            else
            {
                await player.StopAsync();
            }
        }


        public async Task Stop()
        {
            await player.DisconnectAsync();
            lava.Delete(guild);
        }

        private static Random rng = new Random();
        public void Shuffle()
        {
            LavalinkTrack track = Queue[Index];
            Queue.RemoveAt(Index);

            int n = Queue.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                LavalinkTrack value = Queue[k];
                Queue[k] = Queue[n];
                Queue[n] = value;
            }

            Queue.Insert(Index, track);
        }

        public async Task Pause()
        {
            await player.PauseAsync();
            isPaused = true;
        }

        public async Task Resume()
        {
            await player.ResumeAsync();
            isPaused = false;
        }
    }
}
