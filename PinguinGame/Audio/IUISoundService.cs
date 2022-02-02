using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Audio
{
    public interface IUISoundService
    {
        public void PlayCountdownHigh();
        public void PlayCountdownLow();
        public void PlayAccept();
        public void PlayBack();
        public void PlaySelect();
        public void PlayStart();
    }
}
