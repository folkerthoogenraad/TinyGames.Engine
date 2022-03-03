using Microsoft.Xna.Framework;
using PinguinGame.Gameplay.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace PinguinGame.Gameplay.GameObjects.CharacterStates
{
    internal class CharacterSlideState : CharacterState
    {
        public float SlideTimer { get; set; } = 0;
        public Vector2 InitialDirection { get; set; }

        public CharacterSlideState(Vector2 direction)
        {
            InitialDirection = direction.NormalizedOrDefault(new Vector2(1, 0));
        }

        public override void Init(CharacterGameObject character)
        {
            base.Init(character);

            character.Bounce.Height = 4;
            character.Sound.PlayStartSlide();

            character.Physics = character.Physics.StartSlide(InitialDirection * Character.Settings.SlideSpeed);
        }
        public override void Destroy()
        {
            base.Destroy();

            Character.Sound.PlayStopSlide();
        }

        public override CharacterState Update(CharacterGameObject penguin, CharacterInput input, float delta)
        {
            SlideTimer += delta;

            Character.Physics = Character.Physics.Slide(delta, input.MoveDirection);

            if (!input.Action)
            {
                return new CharacterWalkState();
            }

            return this;
        }

        public override void Draw(Graphics2D graphics, CharacterGameObject penguin, CharacterGraphics penguinGraphics)
        {
            var facing = CharacterGraphics.GetFacingFromVector(penguin.Facing);

            penguinGraphics.DrawSlide(graphics, facing, penguin.Position, penguin.Height, SlideTimer);
        }

    }
}
