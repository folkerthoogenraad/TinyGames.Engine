using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using PinguinGame.Gameplay.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Extensions;

namespace PinguinGame.Gameplay
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
    }
}
