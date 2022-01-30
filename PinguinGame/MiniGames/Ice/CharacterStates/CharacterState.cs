using Microsoft.Xna.Framework;
using PinguinGame.MiniGames.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Ice.CharacterStates
{
    internal abstract class CharacterState
    {
        public Character Character { get; set; }
        public virtual void Init(Character character) { Character = character; }
        public virtual void Destroy() { }
        public abstract CharacterState Update(Character character, CharacterInput input, float delta);
        public virtual void Draw(Graphics2D graphics, Character character, CharacterGraphics penguinGraphics) { }

    }
}
