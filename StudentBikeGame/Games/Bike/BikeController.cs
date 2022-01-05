using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Util;

namespace StudentBikeGame.Games.Bike
{
    public class BikeController
    {
        public Bike Bike { get; set; }
        public BikeGraphics BikeGraphics { get; set; }

        public Vector2 Position => Bike.Position;

        public AABB Bounds => AABB.CreateCentered(Position, new Vector2(24, 24));

        private int _steerDirection = 0;
        public float _steering = 0;

        public BikeController(Bike bike, BikeGraphics graphics)
        {
            Bike = bike;
            BikeGraphics = graphics;
        }

        public void Update(BikeWorld world, float delta)
        {
            // TODO input this instead of fetching from global
            var keyState = Keyboard.GetState();

            _steerDirection = 0;
            float acceleration = 0;

            if (keyState.IsKeyDown(Keys.Left))
            {
                _steerDirection += -1;
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                _steerDirection += 1;
            }

            if (keyState.IsKeyDown(Keys.Up))
            {
                acceleration += 1;
            }

            var bikeSurface = world.GetSurfaceType(Bike.Position);
            var handling = world.GetHandlingForSurface(bikeSurface);


            float multiplier = Math.Clamp(1 / (Bike.ForwardVelocity / 140), 2, 6);

            multiplier *= Math.Clamp(Bike.ForwardVelocity / 20, 0, 1);

            //Debug.WriteLine("Forward: " + multiplier);

            _steering = Tools.Lerp(_steering, _steerDirection, delta * 10);

            Bike = Bike.Update(delta, 300 * acceleration, _steering * multiplier, handling);
            BikeGraphics.Update(world, Bike, delta);

        }

        public void Draw(Graphics2D graphics)
        {
            BikeGraphics.Draw(graphics, Bike, _steerDirection);
        }
    }
}
