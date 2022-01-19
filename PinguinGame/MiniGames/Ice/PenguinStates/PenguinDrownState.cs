using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace PinguinGame.MiniGames.Ice.PenguinStates
{
    internal class PenguinDrownState : PenguinState
    {
        private float AnimationTimer = 0;

        public override void Init(Penguin penguin)
        {
            base.Init(penguin);

            penguin.Physics = penguin.Physics.StartSlide(penguin.Position.Normalized() * 16);
        }

        public override PenguinState Update(Penguin penguin, PenguinInput input, float delta)
        {
            AnimationTimer += delta;

            penguin.Physics = penguin.Physics.Move(delta, Vector2.Zero, 1);

            return this;
        }

        public override void Draw(Graphics2D graphics, Penguin penguin, CharacterGraphics penguinGraphics)
        {
            var facing = CharacterGraphics.GetFacingFromVector(penguin.Facing);
            var color = penguin.Player.Color;

            penguinGraphics.DrawDrown(graphics, facing, penguin.DrawPosition, AnimationTimer);
            penguinGraphics.DrawDrownOverlay(graphics, facing, penguin.DrawPosition, AnimationTimer, color);
        }
    }
}
