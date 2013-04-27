using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace LandOfAmbrosia.Managers
{
    class SoundManager : GameComponent
    {
        private List<Song> soundTracks;
        private int unchosenSize;
        private Song currentSong;
        private Random gen;

        public SoundManager(Game game, ContentManager content) : base(game)
        {
            gen = new Random();
            this.loadSounds(content);
            unchosenSize = soundTracks.Count;
            playNextSong();
        }

        private void loadSounds(ContentManager content)
        {
            soundTracks = new List<Song>();
            soundTracks.Add(content.Load<Song>(@"Audio\DragonRider"));
            soundTracks.Add(content.Load<Song>(@"Audio\Unstoppable"));
            soundTracks.Add(content.Load<Song>(@"Audio\Immortal"));
            soundTracks.Add(content.Load<Song>(@"Audio\Invincible"));
            soundTracks.Add(content.Load<Song>(@"Audio\Strength"));
            soundTracks.Add(content.Load<Song>(@"Audio\ToGlory"));
        }

        private void playNextSong()
        {
            //Choses a random index to play, plays it, then swaps that song to the end
            if (soundTracks.Count == 0)
            {
                throw new Exception("No songs to play!");
            }
            //Constant time 'shuffle' alg
            int songIndex = gen.Next(0, unchosenSize);
            currentSong = soundTracks[songIndex];
            MediaPlayer.Play(soundTracks[songIndex]);

            if (--unchosenSize <= 0)
            {
                //Reset
                unchosenSize = soundTracks.Count;
            }
            else
            {
                Song temp = soundTracks[songIndex];
                soundTracks[songIndex] = soundTracks[unchosenSize];
                soundTracks[unchosenSize] = temp;
            }
        }

        //Update will check to see if it's time to play a new song and fade out the old

        public override void Update(GameTime gametime)
        {
            if (currentSong == null || needsTransition(currentSong))
            {
                playNextSong();
            }
        }

        private bool needsTransition(Song currentSong)
        {
            //Check if the current song is about to go off
            TimeSpan currentSongDuration = currentSong.Duration;
            TimeSpan currentSongTimePlayed = MediaPlayer.PlayPosition;
            double secondsLeft = (currentSongDuration - currentSongTimePlayed).TotalSeconds;
            if (secondsLeft <= 0)
            {
                return true;
            }
            return false;
        }
    }
}
