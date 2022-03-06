using Microsoft.Xna.Framework;
using PinguinGame.Graphics;
using PinguinGame.Gameplay.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace PinguinGame.Gameplay.GameObjects.CharacterStates
{
    internal class CharacterGatherSnowState : CharacterState
    {
        private IceGameUIGraphics _ui;
        private float _snowballTimer = 0;
        private float _maxGatherTime = 1.0f;
        public float GatherProgress => _snowballTimer / _maxGatherTime;

        public override void Init(CharacterGameObject character)
        {
            base.Init(character);

            _ui = character.UIGraphics;

            character.Sound.PlaySnowballGather();
        }

        public override CharacterState Update(CharacterGameObject penguin, CharacterInput input, float delta)
        {
            penguin.Physics = penguin.Physics.Move(delta, new Vector2(), penguin.Settings.Acceleration);

            if (!input.ActionSecondary)
            {
                return new CharacterWalkState();
            }

            _snowballTimer += delta;

            if(_snowballTimer > _maxGatherTime)
            {
                penguin.Inventory.HasSnowball = true;

                penguin.Bounce.Height = 4;

                penguin.Sound.PlaySnowballGatherDone();
                return new CharacterWalkState();
            }

            return this;
        }

        public override void Draw(Graphics2D graphics, CharacterGameObject character, CharacterGraphics penguinGraphics)
        {
            var facing = CharacterGraphics.GetFacingFromVector(character.Facing);
            
            penguinGraphics.DrawIdle(graphics, facing, character.Position, character.Height, 0);

            var outline = _ui.SnowballChargeOutline;
            var sprite = _ui.SnowballCharge.GetSpriteNormalized(GatherProgress);

            graphics.DrawSprite(outline, character.Position - new Vector2(0, character.Height + 16), 0, GraphicsHelper.YToDepth(character.Position.Y));
            graphics.DrawSprite(sprite, character.Position - new Vector2(0, character.Height + 16), 0, GraphicsHelper.YToDepth(character.Position.Y), character.Player.Color);
        }
    }
}
