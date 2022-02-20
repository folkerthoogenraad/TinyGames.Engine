using Microsoft.Xna.Framework;
using PinguinGame.MiniGames.Generic;
using PinguinGame.MiniGames.Ice.CharacterActions;
using PinguinGame.MiniGames.Ice.CharacterActions.Data;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace PinguinGame.MiniGames.Ice.CharacterStates
{
    internal class CharacterSlideState : CharacterState
    {
        public SlideData Data { get; set; }

        public CharacterSlideState(Vector2 speed)
        {
            Data = new SlideData()
            {
                SlideTimer = 0,
                InitialDirection = speed,
            };
        }

        public override void Init(CharacterGameObject character)
        {
            base.Init(character);

            character.Bounce.Height = 4;
            character.Sound.PlayStartSlide();

            character.Actions.PushActions();

            character.Actions.CurrentActions = new CharacterActionsSet()
            {
                Move = new SlideAction(character, Data),
                Primary = new StopSlideAction(character),
                Secondary = new NoAction<bool>(character),
            };
        }
        public override void Destroy()
        {
            base.Destroy();

            Character.Sound.PlayStopSlide();

            Character.Actions.PopActions();
        }

        public override CharacterState Update(CharacterGameObject penguin, CharacterInput input, float delta)
        {
            Data.SlideTimer += delta;

            return this;
        }

        public override void Draw(Graphics2D graphics, CharacterGameObject penguin, CharacterGraphics penguinGraphics)
        {
            var facing = CharacterGraphics.GetFacingFromVector(penguin.Facing);
            var color = penguin.Player.Color;

            penguinGraphics.DrawSlide(graphics, facing, penguin.Position, penguin.Height, Data.SlideTimer);
        }

    }
}
