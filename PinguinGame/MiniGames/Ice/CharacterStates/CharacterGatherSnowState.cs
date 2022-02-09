using Microsoft.Xna.Framework;
using PinguinGame.Graphics;
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
        private IceGameUIGraphics _ui;

        public override void Init(Character penguin)
        {
            base.Init(penguin);

            _ui = penguin.Level.UIGraphics;

            penguin.SnowballGathering.RemoveSnowball();
            penguin.Sound.PlaySnowballGather();
        }

        public override CharacterState Update(Character penguin, CharacterInput input, float delta)
        {
            penguin.Physics = penguin.Physics.Move(delta, new Vector2(), penguin.Settings.Acceleration);

            // Maybe disallow stopping with gathering?
            // timeout maybe?
            if (!input.ActionSecondary)
            {
                return new CharacterWalkState();
            }

            penguin.SnowballGathering.Update(delta, input.ActionSecondary);

            if (penguin.SnowballGathering.HasSnowball)
            {
                penguin.Bounce.Height = 4;

                penguin.Sound.PlaySnowballGatherDone();
                return new CharacterWalkState();
            }

            return this;
        }

        public override void Draw(Graphics2D graphics, Character character, CharacterGraphics penguinGraphics)
        {
            var facing = CharacterGraphics.GetFacingFromVector(character.Facing);
            
            penguinGraphics.DrawIdle(graphics, facing, character.Position, character.Height, 0);

            var outline = _ui.SnowballChargeOutline;
            var sprite = _ui.SnowballCharge.GetSpriteNormalized(character.SnowballGathering.GatherProgress);

            graphics.DrawSprite(outline, character.Position - new Vector2(0, character.Height + 16), 0, GraphicsHelper.YToDepth(character.Position.Y));
            graphics.DrawSprite(sprite, character.Position - new Vector2(0, character.Height + 16), 0, GraphicsHelper.YToDepth(character.Position.Y), character.Player.Color);

            //Vector2 barPosition = penguin.Position - new Vector2(0, penguin.Height + 16);

            //graphics.DrawRectangle(AABB.CreateCentered(barPosition, new Vector2(16, 6)), Color.Black);
            //graphics.DrawRectangle(AABB.CreateCentered(barPosition, new Vector2(14 * penguin.SnowballGathering.GatherProgress, 4)), penguin.Player.Color);

            // Draw some indicator?
        }
    }
}
