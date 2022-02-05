using Microsoft.Xna.Framework;
using PinguinGame.MiniGames.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace PinguinGame.MiniGames.Ice.CharacterStates
{
    internal class CharacterDrownState : CharacterState
    {
        private float AnimationTimer = 0;

        public override void Init(Character penguin)
        {
            base.Init(penguin);

            penguin.Bounce.Bouncyness = 0;
        }

        public override CharacterState Update(Character penguin, CharacterInput input, float delta)
        {
            AnimationTimer += delta;

            penguin.Physics = penguin.Physics.Move(delta, Vector2.Zero, 1);

            return this;
        }

        public override void Draw(Graphics2D graphics, Character penguin, CharacterGraphics penguinGraphics)
        {
            var facing = CharacterGraphics.GetFacingFromVector(penguin.Facing);

            if(penguin.Bounce.Height > 0)
            {
                penguinGraphics.DrawIdle(graphics, facing, penguin.Position, penguin.Height, AnimationTimer);
            }
            else
            {
                penguinGraphics.DrawDrown(graphics, facing, penguin.Position, penguin.Height, AnimationTimer);
            }
        }
    }
}
