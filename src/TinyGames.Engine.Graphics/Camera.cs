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
        public AABB LocalBounds => GetLocalBounds();

        public Vector2 Size => new Vector2(Width, Height);

        public float Angle { get; set; }

        public Camera(float height = 1, float aspectRatio = 16.0f / 9.0f)
        {
            Height = height;
            AspectRatio = aspectRatio;
        }

        public Matrix GetMatrix()
        {
            return GetModelMatrix() * GetProjectionMatrix();
        }

        public Matrix GetProjectionMatrix()
        {
            return Matrix.CreateRotationZ(Angle * Tools.DegToRad) *
                Matrix.CreateOrthographicOffCenter(-Width / 2, Width / 2, Height / 2, -Height / 2, -1000, 1000);
        }
        public Matrix GetModelMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-Position, 0));
        }

        public Matrix GetHeightDepthProjectionMatrix()
        {
            var matrix = GetProjectionMatrix();

            matrix.M32 = -matrix.M22;
            matrix.M23 = matrix.M33;

            return matrix;
        }

        public void ClampInBounds(AABB bounds)
        {
            bounds.Left += Width / 2;
            bounds.Right -= Width / 2;

            bounds.Top += Height / 2;
            bounds.Bottom -= Height / 2;

            ClampPositionInBounds(bounds);
        }

        public void ClampPositionInBounds(AABB bounds)
        {
            if (Position.X < bounds.Left) Position.X = bounds.Left;
            if (Position.X > bounds.Right) Position.X = bounds.Right;

            if (Position.Y < bounds.Top) Position.Y = bounds.Top;
            if (Position.Y > bounds.Bottom) Position.Y = bounds.Bottom;

            if (bounds.Width < 0)
            {
                Position.X = bounds.Center.X;
            }
            if (bounds.Height < 0)
            {
                Position.Y = bounds.Center.Y;
            }

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

        private AABB GetLocalBounds()
        {
            return new AABB()
            {
                Left = -Width / 2,
                Right = Width / 2,
                Bottom = Height / 2,
                Top = -Height / 2,
            };
        }

        public Vector2 TransformMousePosition(Vector2 normalizedMousePosition)
        {
            return normalizedMousePosition * Size - Size / 2 + Position;
        }
    }
}
