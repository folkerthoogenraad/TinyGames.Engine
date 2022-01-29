using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace PinguinGame.MiniGames.Generic
{
    internal class PenguinGraphicsForFacing
    {
        public Animation Idle;
        public Animation Walk;
        public Animation Slide;
        public Animation Drown;
    }

    public class CharacterGraphics
    {
        public enum Facing
        {
            Up,
            Down,
            Left,
            Right
        }

        private Dictionary<Facing, PenguinGraphicsForFacing> _graphics;
        private Sprite _shadow;

        public Sprite Shadow => _shadow;

        public CharacterGraphics(Texture2D texture)
        {
            _graphics = new Dictionary<Facing, PenguinGraphicsForFacing>();

            _graphics.Add(Facing.Down, GetGraphicsWithOffset(texture, 0));
            _graphics.Add(Facing.Right, GetGraphicsWithOffset(texture, 16));
            _graphics.Add(Facing.Up, GetGraphicsWithOffset(texture, 32));
            _graphics.Add(Facing.Left, GetGraphicsWithOffset(texture, 48));

            _shadow = new Sprite(texture, new Rectangle(112, 112, 16, 16)).CenterOrigin();
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
            };
        }

        public static Facing GetFacingFromVector(Vector2 v)
        {
            return GetFacingFromAngle(v.GetAngleInDegrees());
        }
        public static Facing GetFacingFromAngle(float angle)
        {
            if (angle > 135) return Facing.Left;
            if (angle > 45) return Facing.Down;
            if (angle > -45) return Facing.Right;
            if (angle > -135) return Facing.Up;

            return Facing.Left;
        }

        internal PenguinGraphicsForFacing GetGraphicsForFacing(Facing facing)
        {
            return _graphics[facing];
        }
    }
}
