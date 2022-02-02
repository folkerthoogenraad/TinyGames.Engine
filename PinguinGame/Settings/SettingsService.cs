using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.IO;

namespace PinguinGame.Settings
{
    public class SettingsService : ISettingsService
    {
        private readonly string SoundSettingsLocation = "Settings/SoundSettings.json";

        private SoundSettings _sound;

        public SettingsService(IStorageSystem storage)
        {
            _sound = storage.Load(SoundSettingsLocation, new SoundSettings());

            storage.Save(SoundSettingsLocation, _sound);
        }

        public SoundSettings GetSoundSettings()
        {
            return _sound;
        }
    }
}
