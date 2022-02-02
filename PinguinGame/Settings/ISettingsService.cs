using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Settings
{
    public interface ISettingsService
    {
        public SoundSettings GetSoundSettings();
    }
}
