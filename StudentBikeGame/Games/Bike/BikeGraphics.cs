using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Util;

namespace StudentBikeGame.Games.Bike
{
    public class BikeGraphicsSettings
    {
        public Sprite BikeSprite;
        public Sprite BikeTurnSprite;
        public Animation BikerAnimation;
        public Animation BikerTurnAnimation;
    }

    public class BikeGraphics
    {
        public BikeGraphicsSettings Settings { get; set; }

        private float _timer = 0;
        private RingList<(Vector2 position, SurfaceType surface)> _backWheel;
        private RingList<(Vector2 position, SurfaceType surface)> _frontWheel;

        public BikeGraphics(BikeGraphicsSettings settings)
        {
            Settings = settings;
            _frontWheel = new RingList<(Vector2 position, SurfaceType surface)>(50);
            _backWheel = new RingList<(Vector2 position, SurfaceType surface)>(50);
        }

        private float PedallingTimer = 0;

        public void Update(BikeWorld world, Bike bike, float delta)
        {
            PedallingTimer += delta * bike.ForwardVelocity * 0.05f;

            _timer -= delta;

            if(_timer < 0)
            {
                var frontWheelPosition = bike.Position + bike.Forward * 10;
                var backWheelPosition = bike.Position - bike.Forward * 10;

                _frontWheel.Add((frontWheelPosition, world.GetSurfaceType(frontWheelPosition)));
                _backWheel.Add((backWheelPosition, world.GetSurfaceType(backWheelPosition)));

                _timer = 0.05f;
            }
        }

        public void Draw(Graphics2D graphics, Bike bike, int steerDirection)
        {
            var angle = bike.Heading.GetAngleInDegrees();

            Vector2 scale;
            Sprite bikeSprite;
            Sprite bikerSprite;

            if (steerDirection == 0)
            {
                scale = new Vector2(1, 1);
                bikeSprite = Settings.BikeSprite;
                bikerSprite = Settings.BikerAnimation.GetSpriteForTime(PedallingTimer);
            }
            else
            {
                scale = new Vector2(1, steerDirection);
                bikeSprite = Settings.BikeTurnSprite;
                bikerSprite = Settings.BikerTurnAnimation.GetSpriteForTime(PedallingTimer);
            }

            graphics.DrawSprite(bikeSprite, bike.Position, scale, angle, 0);
            graphics.DrawSprite(bikerSprite, bike.Position, scale, angle, 0);

            DrawTrail(graphics, _backWheel.Select(x => (x.position, GetSurfaceTrailColor(x.surface))), 4);
            DrawTrail(graphics, _frontWheel.Select(x => (x.position, GetSurfaceTrailColor(x.surface))), 4);
        }

        public Color GetSurfaceTrailColor(SurfaceType surface)
        {
            if (surface == SurfaceType.Grass) return new Color(142, 158, 108);
            if (surface == SurfaceType.Dirt) return new Color(82, 74, 65);

            return new Color(53, 53, 53);
        }

        public void DrawTrail(Graphics2D graphics, IEnumerable<(Vector2 position, Color color)> trail, float width)
        {
            var trailPairs = trail.Reverse().Pairs();

            int index = 0;
            int count = trailPairs.Count();

            float widthPerIndex = width / count;

            foreach (var (from, to) in trailPairs)
            {
                graphics.DrawLine(from.position, to.position, width  - index * widthPerIndex, -1, new Color(0.2f, 0.2f, 0.2f, 0.2f));

                index++;
            }
        }
    }
}
