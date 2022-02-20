using Microsoft.Xna.Framework;
using PinguinGame.MiniGames.Ice.CharacterActions.Data;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Maths;

namespace PinguinGame.MiniGames.Ice.CharacterActions
{
    public class SlideAction : CharacterAction<Vector2>
    {
        public SlideData Data { get; set; }

        public SlideAction(CharacterGameObject character, SlideData slide) : base(character)
        {
            Data = slide;

            Vector2 direction = Data.InitialDirection;

            if (Data.InitialDirection.LengthSquared() > 0)
            {
                direction = Data.InitialDirection;
            }
            else
            {
                direction = new Vector2(1, 0);
            }

            direction = direction.Normalized();

            Character.Physics = Character.Physics.StartSlide(direction * Character.Settings.SlideSpeed);
        }

        public override void Update(float delta, Vector2 status)
        {
            Character.Physics = Character.Physics.Slide(delta, status);
        }
    }
}
