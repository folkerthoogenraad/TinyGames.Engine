using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Audio
{
    public interface IMusicService
    {
        public void PlayMenuMusic();
        public void PlayTitleMusic();
        public void PlayInGameMusic(); // TODO different tracks
        public void PlayVictoryMusic();
    }
}
