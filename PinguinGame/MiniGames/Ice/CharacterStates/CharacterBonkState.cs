using Microsoft.Xna.Framework;
using PinguinGame.Graphics;
using PinguinGame.MiniGames.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Ice.CharacterStates
{
    internal class CharacterBonkState : CharacterState
    {
        public Vector2 Velocity { get; set; }
        private float BonkTimer = 0;

        private IceGameEffects _effects;

        public CharacterBonkState(IceGameEffects effects, Vector2 velocity, float timeout = 1)
        {
            Velocity = velocity;
            BonkTimer = timeout;
            _effects = effects;
        }

        public override void Init(CharacterGameObject penguin)
        {
            base.Init(penguin);

            penguin.Bounce.Height = 4;
            penguin.Physics = penguin.Physics.StartBonk(Velocity);
        }

        public override CharacterState Update(CharacterGameObject penguin, CharacterInput input, float delta)
        {
            BonkTimer -= delta;

            if(BonkTimer < 0)
            {
                return new CharacterWalkState();
            }

            penguin.Physics = penguin.Physics.Move(delta, Vector2.Zero, penguin.Settings.BonkSlowdown);

            return this;
        }

        public override void Draw(Graphics2D graphics, CharacterGameObject character, CharacterGraphics characterGraphics)
        {
            var facing = CharacterGraphics.GetFacingFromVector(character.Facing);

            characterGraphics.DrawIdle(graphics, facing, character.Position, character.Height, BonkTimer);

            graphics.DrawSprite(_effects.StunEffect.GetSpriteForTime(BonkTimer * 0.5f), character.Position - new Vector2(0, character.Bounce.Height + character.GroundHeight + 8), 0, GraphicsHelper.YToDepth(character.Position.Y + 1));
        }
    }
}
