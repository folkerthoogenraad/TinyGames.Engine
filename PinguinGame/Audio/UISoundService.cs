using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Audio
{
    // TODO this should create its own content manager, because then we can easily load and unload everything
    public class UISoundService :  IUISoundService
    {
        public SoundEffect CountDownHigh;
        public SoundEffect CountDownLow;

        public SoundEffect Accept;
        public SoundEffect Back;
        public SoundEffect Select;
        public SoundEffect Start;

        public UISoundService(ContentManager content)
        {
            CountDownHigh = content.Load<SoundEffect>("SoundEffects/UI/Countdown_High");
            CountDownLow = content.Load<SoundEffect>("SoundEffects/UI/Countdown_Low");

            Accept = content.Load<SoundEffect>("SoundEffects/UI/Accept");
            Back = content.Load<SoundEffect>("SoundEffects/UI/Back");
            Select = content.Load<SoundEffect>("SoundEffects/UI/Select");
            Start = content.Load<SoundEffect>("SoundEffects/UI/Start");
        }

        public void PlayCountdownHigh()
        {
            CountDownHigh.Play();
        }

        public void PlayCountdownLow()
        {
            CountDownLow.Play();
        }

        public void PlayAccept()
        {
            Accept.Play(0.5f, 1, 0);
        }

        public void PlayBack()
        {
            Back.Play(0.5f, 1, 0);
        }

        public void PlaySelect()
        {
            Select.Play(0.5f, 1, 0);
        }
        public void PlayStart()
        {
            Start.Play();
        }
    }
}
