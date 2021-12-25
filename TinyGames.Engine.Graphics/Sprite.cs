using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TinyGames.Engine.Maths;

namespace TinyGames.Engine.Graphics
{
    public class Sprite
    {
        public Texture2D Texture;

        public Vector2 Origin;
        public Rectangle SourceRectangle;

        public Sprite(Texture2D texture) : this(texture, new Rectangle(0, 0, texture.Width, texture.Height)) { }

        public Sprite(Texture2D texture, Rectangle rectangle)
        {
            Texture = texture;
            Origin = new Vector2(0, 0);
            SourceRectangle = rectangle;
        }

        public Sprite CenterOrigin()
        {
            Origin = Size / 2;
            return this;
        }

        public Sprite SetOrigin(Vector2 origin)
        {
            Origin = origin;
            return this;
        }

        public Sprite SubSprite(int x, int y, int w, int h)
        {
            return new Sprite(Texture, new Rectangle(
                SourceRectangle.X + x,
                SourceRectangle.Y + y,
                w, 
                h));
        }

        public Vector2 Size
        {
            get
            {
                return new Vector2(Width, Height);
            }
        }

        public float Width { get { return SourceRectangle.Width; } }
        public float Height { get { return SourceRectangle.Height; } }

        public Color[] GetData()
        {
            Color[] colors = new Color[SourceRectangle.Width * SourceRectangle.Height];

            GetData(colors);

            return colors;
        }

        public void GetData(Color[] colors)
        {
            if (colors.Length != Width * Height) throw new Exception("Length of array should be width * height for the sprite");

            Texture.GetData(0, SourceRectangle, colors, 0, SourceRectangle.Width * SourceRectangle.Height);
        }

        public AABB BoundingBox
        {
            get
            {
                return new AABB()
                {
                    Left = -Origin.X,
                    Top = -Origin.Y,
                    Right = Width - Origin.X,
                    Bottom = Height - Origin.Y
                };
            }
        }

        public AABB UVRect
        {
            get
            {
                return new AABB()
                {
                    Left = SourceRectangle.Left / (float)Texture.Width,
                    Top = SourceRectangle.Top / (float)Texture.Height,
                    Right = SourceRectangle.Right / (float)Texture.Width,
                    Bottom = SourceRectangle.Bottom / (float)Texture.Height,
                };
            }
        }

        public static bool IsEmpty(Color[] colors)
        {
            for(int i = 0; i < colors.Length; i++)
            {
                if (colors[i].A != 0) return false;
            }
            return true;
        }
    }
}
