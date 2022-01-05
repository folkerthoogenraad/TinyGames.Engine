using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StudentBikeGame.Games.Bike;
using StudentBikeGame.Games.Obstacles;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace StudentBikeGame.Games
{
    public class BikeWorld
    {
        public TextureSampler CollisionSampler;
        public Sprite Background;

        public DataMapping<Color, SurfaceType> SurfaceMapping;
        public DataMapping<SurfaceType, SurfaceHandling> HandlingMapping;


        public BikeWorld(Texture2D background, Texture2D backgroundCollision, CoinGraphics coinGraphics)
        {
            CollisionSampler = TextureSampler.FromTexture(backgroundCollision);
            Background = new Sprite(background);

            var grassSurface = new SurfaceHandling()
            {
                SteepnessMuliplier = 1f,
                GripMultiplier = 0.4f,
                ResistanceMultiplier = 2f,
            };

            var dirtSurface = new SurfaceHandling()
            {
                SteepnessMuliplier = 1,
                GripMultiplier = 0.6f,
                ResistanceMultiplier = 1f,
            };
            var asphaltSurface = new SurfaceHandling()
            {
                SteepnessMuliplier = 40,
                GripMultiplier = 1.2f,
                ResistanceMultiplier = 1.0f,
            };

            SurfaceMapping = new DataMapping<Color, SurfaceType>(SurfaceType.Grass);
            SurfaceMapping.Register(Color.Red, SurfaceType.Dirt);
            SurfaceMapping.Register(Color.Blue, SurfaceType.Asphalt);

            HandlingMapping = new DataMapping<SurfaceType, SurfaceHandling>(grassSurface);
            HandlingMapping.Register(SurfaceType.Dirt, dirtSurface);
            HandlingMapping.Register(SurfaceType.Asphalt, asphaltSurface);

        }

        public SurfaceType GetSurfaceType(Vector2 postion)
        {
            return SurfaceMapping.Map(CollisionSampler.GetColor(postion));
        }

        public SurfaceHandling GetHandlingForSurface(SurfaceType surface)
        {
            return HandlingMapping.Map(surface);
        }

        public void Draw(Graphics2D graphics)
        {
            graphics.DrawSprite(Background, Vector2.Zero, 0, -2);

        }
    }
}
