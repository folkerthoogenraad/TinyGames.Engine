using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace PinguinGame.Gameplay
{
    public class PenguinGraphicsForFacing
    {
        public Animation Idle;
        public Animation Walk;
        public Animation Slide;
        public Animation Drown;

        public Sprite Shadow;
    }

    public enum GraphicsFacing
    {
        Up,
        Down,
        Left,
        Right
    }

    public class GraphicsForFacing<T>
    {
        private Dictionary<GraphicsFacing, T> _graphics;

        public GraphicsForFacing()
        {
            _graphics = new Dictionary<GraphicsFacing, T>();
        }
        public T GetGraphicsForFacing(GraphicsFacing facing)
        {
            return _graphics[facing];
        }
        public void RegisterGraphicsForFacing(GraphicsFacing facing, T graphics)
        {
            _graphics[facing] = graphics;
        }
        public static GraphicsFacing GetFacingFromVector(Vector2 v)
        {
            return GetFacingFromAngle(v.GetAngleInDegrees());
        }
        public static GraphicsFacing GetFacingFromAngle(float angle)
        {
            if (angle > 135) return GraphicsFacing.Left;
            if (angle > 45) return GraphicsFacing.Down;
            if (angle > -45) return GraphicsFacing.Right;
            if (angle > -135) return GraphicsFacing.Up;

            return GraphicsFacing.Left;
        }
    }

    public class CharacterGraphics : GraphicsForFacing<PenguinGraphicsForFacing>
    {
        public Sprite Shadow => GetGraphicsForFacing(GraphicsFacing.Up).Shadow;

        public CharacterGraphics(Texture2D texture)
        {
            RegisterGraphicsForFacing(GraphicsFacing.Down, GetGraphicsWithOffset(texture, 0));
            RegisterGraphicsForFacing(GraphicsFacing.Right, GetGraphicsWithOffset(texture, 16));
            RegisterGraphicsForFacing(GraphicsFacing.Up, GetGraphicsWithOffset(texture, 32));
            RegisterGraphicsForFacing(GraphicsFacing.Left, GetGraphicsWithOffset(texture, 48));
        }
        private PenguinGraphicsForFacing GetGraphicsWithOffset(Texture2D texture, int xoffset)
        {
            return new PenguinGraphicsForFacing()
            {
                Idle = Animation.FromSprites(6,
                    new Sprite(texture, new Rectangle(xoffset, 0, 16, 16)).SetOrigin(8, 16)
                    ),

                Walk = Animation.FromSprites(6,
                    new Sprite(texture, new Rectangle(xoffset, 16, 16, 16)).SetOrigin(8, 16),
                    new Sprite(texture, new Rectangle(xoffset, 32, 16, 16)).SetOrigin(8, 16)
                    ),

                Slide = Animation.FromSprites(6,
                    new Sprite(texture, new Rectangle(xoffset, 48, 16, 16)).SetOrigin(8, 16),
                    new Sprite(texture, new Rectangle(xoffset, 64, 16, 16)).SetOrigin(8, 16)
                    ),
                Drown = Animation.FromSprites(3,
                    new Sprite(texture, new Rectangle(xoffset, 80, 16, 16)).SetOrigin(8, 16),
                    new Sprite(texture, new Rectangle(xoffset, 96, 16, 16)).SetOrigin(8, 16)
                    ),
                Shadow = new Sprite(texture, new Rectangle(112, 112, 16, 16)).CenterOrigin(),
            };
        }
    }


    // TODO move these classes to files :)
    public class ShoppingCartGraphicsForFacing
    {
        public Sprite Cart;
        public Sprite CartOverlay;
    }

    public class ShoppingCartGraphics : GraphicsForFacing<ShoppingCartGraphicsForFacing>
    {
        public ShoppingCartGraphics(Texture2D texture)
        {
            RegisterGraphicsForFacing(GraphicsFacing.Down, GetGraphicsWithOffset(texture, 0));
            RegisterGraphicsForFacing(GraphicsFacing.Up, GetGraphicsWithOffset(texture, 24));
            RegisterGraphicsForFacing(GraphicsFacing.Right, GetGraphicsWithOffset(texture, 48));
            RegisterGraphicsForFacing(GraphicsFacing.Left, GetGraphicsWithOffset(texture, 48 + 24));
        }
        private ShoppingCartGraphicsForFacing GetGraphicsWithOffset(Texture2D texture, int xoffset)
        {
            return new ShoppingCartGraphicsForFacing()
            {
                Cart = new Sprite(texture, new Rectangle(xoffset, 0, 24, 24)).SetOrigin(12, 22),
                CartOverlay = new Sprite(texture, new Rectangle(xoffset, 24, 24, 24)).SetOrigin(12, 22),
            };
        }

    }
}
