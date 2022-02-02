using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using PinguinGame.MiniGames.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Extensions;

namespace PinguinGame.MiniGames.Ice
{
    public class CharacterSound
    {
        public SoundEffect Footstep { get; set; }
        public SoundEffect Bonk { get; set; }

        public SoundEffect Slide { get; set; }
        public SoundEffect SlideHold { get; set; }
        public SoundEffect Splash { get; set; }

        public SoundEffect SnowballThrow { get; set; }
        public SoundEffect SnowballGather { get; set; }
        public SoundEffect SnowballGatherDone { get; set; }
        public SoundEffect SnowballHit { get; set; }

        private float Timer;
        private Random _random;
        private bool _wasDrowning = false;

        private SoundEffectInstance _slideSoundInstance;

        internal CharacterSound() {
            _random = new Random();
        }

        public void PlayStartSlide()
        {
            Slide.Play(1, RandomPitch(), 0);
            _slideSoundInstance = SlideHold.CreateInstance();
            _slideSoundInstance.IsLooped = true;
            _slideSoundInstance.Play();
        }
        public void PlayStopSlide()
        {
        }

        public void PlayStartWalking()
        {
        }

        public void PlayStopWalking()
        {
        }

        public void PlayBonk()
        {
            Bonk?.Play(1f, RandomPitch(), 0);
        }

        public void PlaySnowHit()
        {
            SnowballHit?.Play(1f, RandomPitch(), 0);
        }
        public void PlaySnowballThrow()
        {
            SnowballThrow?.Play(1f, RandomPitch(), 0);
        }
        public void PlaySnowballGather()
        {
            SnowballGather?.Play(1f, RandomPitch(), 0);
        }
        public void PlaySnowballGatherDone()
        {
            SnowballGatherDone?.Play(1f, RandomPitch(), 0);
        }

        public void Update(Character character, float delta)
        {
            if(_slideSoundInstance != null && !character.IsSliding && _slideSoundInstance.State == SoundState.Playing)
            {
                _slideSoundInstance.Stop();
            }

            if (character.IsWalking)
            {
                Timer += delta * (character.Physics.Velocity.Length() / 12);

                if(Timer > 1)
                {
                    Timer -= 1;
                    Footstep?.Play(1f, RandomPitch(), 0);
                }
            }

            if (character.IsDrowning && character.Bounce.Height <= 0 && !_wasDrowning)
            {
                Splash?.Play(1f, RandomPitch(), 0);
                _wasDrowning = true;
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
                Splash = content.Load<SoundEffect>("SoundEffects/Characters/Splash"),

                SnowballHit = content.Load<SoundEffect>("SoundEffects/Characters/SnowballHit"),
                SnowballGather = content.Load<SoundEffect>("SoundEffects/Characters/SnowballGather"),
                SnowballGatherDone = content.Load<SoundEffect>("SoundEffects/Characters/SnowballGatherDone"),
                SnowballThrow = content.Load<SoundEffect>("SoundEffects/Characters/SnowballThrow"),

            };
        }

        private float RandomPitch()
        {
            return _random.NextFloatRange(-0.1f, 0.1f);
        }
    }
}
