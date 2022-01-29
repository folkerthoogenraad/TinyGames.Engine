using Microsoft.Xna.Framework;
using PinguinGame.MiniGames.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace PinguinGame.MiniGames.Ice.CharacterStates
{
    internal class CharacterGatherSnowState : CharacterState
    {
        public override void Init(Character penguin)
        {
            base.Init(penguin);

            penguin.CharacterSnowball.RemoveSnowball();
        }

        public override CharacterState Update(Character penguin, CharacterInput input, float delta)
        {
            penguin.Physics = penguin.Physics.Move(delta, new Vector2(), penguin.Settings.Acceleration);

            if (!input.GatherSnow)
            {
                return new CharacterWalkState();
            }

            penguin.CharacterSnowball.Update(delta, input.GatherSnow);

            if (penguin.CharacterSnowball.HasSnowball)
            {
                penguin.Bounce.Height = 4;
                return new CharacterWalkState();
            }

            return this;
        }

        public override void Draw(Graphics2D graphics, Character penguin, CharacterGraphics penguinGraphics)
        {
            var facing = CharacterGraphics.GetFacingFromVector(penguin.Facing);
            
            penguinGraphics.DrawIdle(graphics, facing, penguin.DrawPosition, 0);

            Vector2 barPosition = penguin.DrawPosition - new Vector2(0, 16);

            graphics.DrawRectangle(AABB.CreateCentered(barPosition, new Vector2(16, 6)), Color.Black);
            graphics.DrawRectangle(AABB.CreateCentered(barPosition, new Vector2(14 * penguin.CharacterSnowball.GatherProgress, 4)), penguin.Player.Color);

            // Draw some indicator?
        }
    }
}
