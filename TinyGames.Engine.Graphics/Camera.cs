using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Maths;

namespace TinyGames.Engine.Graphics
{
    public class Camera
    {
        public float AspectRatio { get; set; }
        public float Height { get; set; }
        public float Width => Height * AspectRatio;

        public Vector2 Position;
        public AABB Bounds => GetBounds();

        public Vector2 Size => new Vector2(Width, Height);

        public float Angle { get; set; }

        public Camera(float height = 1, float aspectRatio = 16.0f / 9.0f)
        {
            Height = height;
            AspectRatio = aspectRatio;
        }

        public Matrix GetMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-Position, 0)) * 
                Matrix.CreateRotationZ(Angle * Tools.DegToRad) *
                Matrix.CreateOrthographicOffCenter(- Width / 2, Width / 2, Height / 2, -Height / 2, -1000, 1000);
        }

        private AABB GetBounds()
        {
            return new AABB()
            {
                Left = Position.X - Width / 2,
                Right = Position.X + Width / 2,
                Bottom = Position.Y + Height / 2,
                Top = Position.Y - Height / 2,
            };
        }

        public Vector2 TransformMousePosition(Vector2 normalizedMousePosition)
        {
            return normalizedMousePosition * Size - Size / 2 + Position;
        }
    }
}
