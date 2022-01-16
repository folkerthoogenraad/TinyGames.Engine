using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Audio
{
    public class MusicService : IMusicService
    {
        public Song InGame;
        public Song Title;
        public Song Menu;
        public Song Victory;

        public Song CurrentSong = null;

        public MusicService(ContentManager content)
        {
            InGame = content.Load<Song>("Songs/InGame");
            Title = content.Load<Song>("Songs/Title");
            Menu = content.Load<Song>("Songs/Menu");
            Victory = content.Load<Song>("Songs/Victory");
        }

        public void PlayInGameMusic()
        {
            SetSong(InGame);
        }

        public void PlayMenuMusic()
        {
            SetSong(Menu);
        }

        public void PlayTitleMusic()
        {
            SetSong(Title);
        }

        public void PlayVictoryMusic()
        {
            SetSong(Victory);
        }

        public void SetSong(Song song)
        {
            if (CurrentSong == song) return;

            MediaPlayer.Stop(); 

            MediaPlayer.IsRepeating = true;
            //MediaPlayer.Play(song);

            CurrentSong = song;
        }
    }
}
