using Microsoft.Xna.Framework;
using PinguinGame.MiniGames.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Ice.CharacterStates
{
    public abstract class CharacterState
    {
        public CharacterGameObject Character { get; set; }
        public virtual void Init(CharacterGameObject character) { Character = character; }
        public virtual void Destroy() { }
        public abstract CharacterState Update(CharacterGameObject character, CharacterInput input, float delta);
        public virtual void Draw(Graphics2D graphics, CharacterGameObject character, CharacterGraphics penguinGraphics) { }

    }
}
