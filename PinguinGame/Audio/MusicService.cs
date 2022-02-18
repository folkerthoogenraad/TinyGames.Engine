using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using PinguinGame.Settings;
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

        private readonly ISettingsService _settings;
        private readonly SoundSettings _soundSettings;

        public MusicService(ContentManager content, ISettingsService settings)
        {
            InGame = content.Load<Song>("Songs/InGame");
            Title = content.Load<Song>("Songs/Title");
            Menu = content.Load<Song>("Songs/Menu");
            Victory = content.Load<Song>("Songs/Victory");

            _settings = settings;
            _soundSettings = _settings.GetSoundSettings();
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

            MediaPlayer.Volume = _soundSettings.MusicVolume;

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);

            CurrentSong = song;
        }
    }
}
