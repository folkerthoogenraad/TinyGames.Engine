using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using PinguinGame.MiniGames.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Ice
{
    public class CharacterSound
    {
        public SoundEffect Footstep { get; set; }
        public SoundEffect Bonk { get; set; }

        public SoundEffect Slide { get; set; }
        public SoundEffect SlideHold { get; set; }

        private bool Walking = false;
        private float Timer;

        internal CharacterSound() { }

        public void StartSlide()
        {
            Slide.Play();
        }
        public void StopSlide()
        {

        }

        public void StartWalking()
        {
            Walking = true;
        }

        public void StopWalking()
        {
            Walking = false;
        }

        public void Update(float delta)
        {
            Timer -= delta;

            if(Timer < 0)
            {
                Timer += 0.3f;

                if (Walking) Footstep?.Play();
            }
        }

        public static CharacterSound CreateCharacterSound(ContentManager content)
        {
            return new CharacterSound()
            {
                Footstep = content.Load<SoundEffect>("SoundEffects/Characters/Footstep"),
                Bonk = content.Load<SoundEffect>("SoundEffects/Characters/Bonk"),

                Slide = content.Load<SoundEffect>("SoundEffects/Characters/Slide"),
                SlideHold = content.Load<SoundEffect>("SoundEffects/Characters/SlideHold"),
            };
        }
    }
}
