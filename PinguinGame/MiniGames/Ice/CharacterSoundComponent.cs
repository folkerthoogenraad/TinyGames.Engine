using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Extensions;

namespace PinguinGame.MiniGames.Ice
{
    public class CharacterSoundComponent
    {
        public CharacterSound Resources { get; set; }
        private float Timer;
        private Random _random;
        private bool _wasDrowning = false;

        private SoundEffectInstance _slideSoundInstance;

        public CharacterSoundComponent(CharacterSound sound)
        {
            Resources = sound;
            _random = new Random();
        }

        public void PlayStartSlide()
        {
            Resources.Slide.Play(1, RandomPitch(), 0);
            _slideSoundInstance = Resources.SlideHold.CreateInstance();
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
            Resources.Bonk?.Play(1f, RandomPitch(), 0);
        }

        public void PlaySnowHit()
        {
            Resources.SnowballHit?.Play(1f, RandomPitch(), 0);
        }
        public void PlaySnowballThrow()
        {
            Resources.SnowballThrow?.Play(1f, RandomPitch(), 0);
        }
        public void PlaySnowballGather()
        {
            Resources.SnowballGather?.Play(1f, RandomPitch(), 0);
        }
        public void PlaySnowballGatherDone()
        {
            Resources.SnowballGatherDone?.Play(1f, RandomPitch(), 0);
        }

        public void StopAll()
        {
            _slideSoundInstance?.Stop();
        }

        public void Update(Character character, float delta)
        {
            if (_slideSoundInstance != null && !character.IsSliding && _slideSoundInstance.State == SoundState.Playing)
            {
                _slideSoundInstance.Stop();
            }

            if (character.IsWalking)
            {
                Timer += delta * (character.Physics.Velocity.Length() / 12);

                if (Timer > 1)
                {
                    Timer -= 1;
                    Resources.Footstep?.Play(1f, RandomPitch(), 0);
                }
            }

            if (character.IsDrowning)
            {
                if (character.Bounce.Height <= 0 && !_wasDrowning)
                {
                    Resources.Splash?.Play(1f, RandomPitch(), 0);
                    _wasDrowning = true;
                }
            }

        }

        private float RandomPitch()
        {
            return _random.NextFloatRange(-0.1f, 0.1f);
        }
    }
}
