using Microsoft.Xna.Framework;
using PinguinGame.Gameplay.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace PinguinGame.Gameplay.GameObjects.CharacterStates
{
    internal class CharacterVehicleState : CharacterState
    {
        public ShoppingCartGameObject Cart { get; set; }

        public CharacterVehicleState(ShoppingCartGameObject cart)
        {
            Cart = cart;
        }

        public override void Init(CharacterGameObject character)
        {
            base.Init(character);

            Cart.Controller = character;

            SyncCharacterPosition();
        }
        public override void Destroy()
        {
            base.Destroy();

            if(Cart.Controller == Character)
            {
                Cart.Controller = null;
            }
        }

        public override CharacterState Update(CharacterGameObject character, CharacterInput input, float delta)
        {
            if (Cart.Controller != Character)
            {
                return new CharacterWalkState();
            }

            Cart.Velocity += input.MoveDirection * 128 * delta;

            if(input.MoveDirection.LengthSquared() > 0)
            {
                character.Physics.Facing = input.MoveDirection;
                Cart.Facing = input.MoveDirection;
            }

            SyncCharacterPosition();
            return this;
        }

        public override void Draw(Graphics2D graphics, CharacterGameObject character, CharacterGraphics characterGraphics)
        {
            var facing = CharacterGraphics.GetFacingFromVector(Cart.Facing);
            characterGraphics.DrawIdle(graphics, facing, character.Position, character.Height, 0);
        }

        private void SyncCharacterPosition()
        {
            Character.Position = Cart.Position;
        }

    }
}
