using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TinyGames.Engine.Maths
{
    public struct AABB
    {
        public float Left;
        public float Right;
        public float Top;
        public float Bottom;

        public Vector2 Center
        {
            get
            {
                return new Vector2((Left + Right) / 2, (Top + Bottom) / 2);
            }
        }
        public Vector2 TopCenter
        {
            get
            {
                return new Vector2((Left + Right) / 2, Top);
            }
        }
        public Vector2 BottomCenter
        {
            get
            {
                return new Vector2((Left + Right) / 2, Bottom);
            }
        }
        public Vector2 RightCenter
        {
            get
            {
                return new Vector2(Right, (Top + Bottom) / 2);
            }
        }
        public Vector2 LeftCenter
        {
            get
            {
                return new Vector2(Left, (Top + Bottom) / 2);
            }
        }

        public Vector2 Size
        {
            get
            {
                return new Vector2(Width, Height);
            }
        }
        public Vector2 Position
        {
            get
            {
                return new Vector2(Left, Top);
            }
        }
        public float Diameter
        {
            get
            {
                return Size.Length();
            }
        }

        public Vector2 TopLeft => new Vector2(Left, Top);
        public Vector2 BottomLeft => new Vector2(Left, Bottom);
        public Vector2 TopRight => new Vector2(Right, Top);
        public Vector2 BottomRight => new Vector2(Right, Bottom);

        public float Width => Right - Left;
        public float Height => Bottom - Top;

        public float X => Left;
        public float Y => Top;

        public void Translate(Vector2 amount)
        {
            Left += amount.X;
            Right += amount.X;

            Top += amount.Y;
            Bottom += amount.Y;
        }
        public AABB Translated(Vector2 amount)
        {
            var c = Clone();

            c.Translate(amount);

            return c;
        }

        public AABB Grow(float amount)
        {
            return new AABB()
            {
                Left = Left - amount,
                Right = Right + amount,
                Top = Top - amount,
                Bottom = Bottom + amount
            };
        }
        public AABB Shrink(float amount)
        {
            return Grow(-amount);
        }
        
        public Vector2 ClampPoint(Vector2 input)
        {
            if (input.X < Left) input.X = Left;
            if (input.X > Right) input.X = Right;

            if (input.Y < Top) input.Y = Top;
            if (input.Y > Bottom) input.Y = Bottom;

            return input;
        }

        public float DistanceTo(Vector2 point)
        {
            return (point - ClampPoint(point)).Length();
        }

        public bool Contains(Vector2 point)
        {
            if (point.X <= Left) return false;
            if (point.X >= Right) return false;
            if (point.Y <= Top) return false;
            if (point.Y >= Bottom) return false;

            return true;
        }

        public Vector2 NormalAt(Vector2 v)
        {
            // Initial: Left
            Vector2 normal = new Vector2(-1, 0);
            float distance = MathF.Abs(v.X - Left);

            // Right
            {
                float d = MathF.Abs(v.X - Right);
                if (d < distance)
                {
                    distance = d;
                    normal = new Vector2(1, 0);
                }
            }

            // Top
            {
                float d = MathF.Abs(v.Y - Top);
                if (d < distance)
                {
                    distance = d;
                    normal = new Vector2(0, -1);
                }
            }

            // Bottom
            {
                float d = MathF.Abs(v.Y - Bottom);
                if (d < distance)
                {
                    distance = d;
                    normal = new Vector2(0, 1);
                }
            }

            return normal;
        }

        public bool RayIntersection(Vector2 origin, Vector2 direction, float maxDistance = 1)
        {
            return RayIntersection(origin, direction, out _, out _, maxDistance);
        }

        public bool RayIntersection(Vector2 origin, Vector2 direction, out float distance, out Vector2 normal, float maxDistance = 1)
        {
            float tmin = 0;
            float tmax = maxDistance;

            {
                float tx1 = (Left - origin.X) / direction.X;
                float tx2 = (Right - origin.X) / direction.X;

                tmin = MathF.Max(tmin, MathF.Min(tx1, tx2));
                tmax = MathF.Min(tmax, MathF.Max(tx1, tx2));
            }

            {
                float tx1 = (Top - origin.Y) / direction.Y;
                float tx2 = (Bottom - origin.Y) / direction.Y;

                tmin = MathF.Max(tmin, MathF.Min(tx1, tx2));
                tmax = MathF.Min(tmax, MathF.Max(tx1, tx2));
            }

            if(tmax > tmin)
            {
                distance = tmin;
                normal = NormalAt(origin + direction * distance);
                return true;
            }

            normal = new Vector2(0, 0);
            distance = maxDistance;
            return false;
        }

        public bool BoxCast(AABB other, Vector2 direction, out float distance, out Vector2 normal, float maxDistance = 1)
        {
            var diff = MinkowskiDifference(other, this);

            return diff.RayIntersection(Vector2.Zero, direction, out distance, out normal, maxDistance);
        }

        public Vector2 UnstuckVector(AABB other, Vector2 direction)
        {
            // TODO this is so ugly :)
            if (direction.X == 1) return new Vector2(other.Right - Left, 0);
            if (direction.X == -1) return new Vector2(other.Left - Right, 0);
            if (direction.Y == 1) return new Vector2(0, other.Bottom - Top);
            if (direction.Y == -1) return new Vector2(0, other.Top - Bottom);

            return new Vector2(0, 0);
        }

        public Vector2 Unstuck(Vector2 point)
        {
            Vector2 horizontalAxis = new Vector2();
            Vector2 verticalAxis = new Vector2();
            float horizontalDistance = 0;
            float verticalDistance = 0;

            // Horizontal
            float leftDistance = point.X - Left;
            float rightDistance = Right - point.X;

            if (leftDistance < rightDistance)
            {
                horizontalDistance = leftDistance;
                horizontalAxis.X = -1;
            }
            else
            {
                horizontalDistance = rightDistance;
                horizontalAxis.X = 1;
            }

            // Vertical
            float topDistance = point.Y - Top;
            float bottomDistance = Bottom - point.Y;

            if (topDistance < bottomDistance)
            {
                verticalDistance = topDistance;
                verticalAxis.Y = -1;
            }
            else
            {
                verticalDistance = bottomDistance;
                verticalAxis.Y = 1;
            }

            if(horizontalDistance > verticalDistance) return verticalDistance * verticalAxis;
            else return horizontalDistance * horizontalAxis;
        }

        public AABB Clone()
        {
            AABB t = this;

            return t;
        }

        public bool Overlaps(AABB other)
        {
            return Overlaps(this, other);
        }

        // Note: On the edges is _no_ overlap. 
        public static bool Overlaps(AABB a, AABB b)
        {
            if (a.Right <= b.Left) return false;
            if (a.Left >= b.Right) return false;

            if (a.Bottom <= b.Top) return false;
            if (a.Top >= b.Bottom) return false;

            return true;
        }
        public static bool OverlapX(AABB a, AABB b)
        {
            if (a.Right <= b.Left) return false;
            if (a.Left >= b.Right) return false;

            return true;
        }
        public static bool OverlapY(AABB a, AABB b)
        {
            if (a.Bottom <= b.Top) return false;
            if (a.Top >= b.Bottom) return false;

            return true;
        }

        // Is the other on the right of self
        public static bool IsOnRight(AABB self, AABB other)
        {
            return self.Right < other.Left;
        }
        // Is the other on the left of self
        public static bool IsOnLeft(AABB self, AABB other)
        {
            return self.Left > other.Right;
        }
        // Is the other on top of self
        public static bool IsAbove(AABB self, AABB other)
        {
            return self.Top > other.Bottom;
        }
        // Is the other on bottom of self
        public static bool IsBelow(AABB self, AABB other)
        {
            return self.Bottom < other.Top;
        }

        public static float MoveX(AABB a, AABB b, float move)
        {
            if (move == 0) return 0;

            if (!OverlapY(a, b)) return move;

            if (move > 0)
            {
                float distance = b.Left - a.Right;

                if (distance < 0) return move;

                if (distance < move) return distance;
            }

            if (move < 0)
            {
                float distance = b.Right - a.Left;

                if (distance > 0) return move;

                if (distance > move) return distance;
            }

            return move;
        }
        public static float MoveY(AABB a, AABB b, float move)
        {
            if (move == 0) return 0;

            if (!OverlapX(a, b)) return move;

            if (move > 0)
            {
                float distance = b.Top - a.Bottom;

                if (distance < 0) return move;

                if (distance < move) return distance;
            }

            if (move < 0)
            {
                float distance = b.Bottom - a.Top;

                if (distance > 0) return move;

                if (distance > move) return distance;
            }

            return move;
        }

        public static AABB MinkowskiDifference(AABB a, AABB b)
        {
            return Create(
                a.Left - b.Right,
                a.Top - b.Bottom,
                a.Width + b.Width, 
                a.Height + b.Height
                );
        }

        public static AABB operator +(AABB a, Vector2 b)
        {
            a.Translate(b);
            return a;
        }

        public static AABB Lerp(AABB from, AABB to, float factor)
        {
            return new AABB() {
                Left = Tools.Lerp(from.Left, to.Left, factor),
                Right = Tools.Lerp(from.Right, to.Right, factor),
                Top = Tools.Lerp(from.Top, to.Top, factor),
                Bottom = Tools.Lerp(from.Bottom, to.Bottom, factor)
            };
        }

        public override bool Equals(object obj)
        {
            if(obj is AABB)
            {
                var a = (AABB)obj;

                if (a.Left != Left) return false;
                if (a.Right != Right) return false;
                if (a.Top != Top) return false;
                if (a.Bottom != Bottom) return false;

                return true;
            }
            return false;
        }

        public Rectangle ToRectangle()
        {
            int left = (int)Left;
            int right = (int)Right;
            int top = (int)Top;
            int bottom = (int)Bottom;

            return new Rectangle(left, top, right - left, bottom - top);
        }

        public static AABB Create(float x, float y, float w, float h)
        {
            return new AABB()
            {
                Left = x,
                Top = y,
                Right = x + w,
                Bottom = y + h
            };
        }

        public static AABB CreateCentered(float x, float y, float w, float h)
        {
            return new AABB()
            {
                Left = x - w / 2,
                Top = y - h / 2,
                Right = x + w / 2,
                Bottom = y + h / 2
            };
        }
        public static AABB Create(Vector2 position, Vector2 size)
        {
            return Create(position.X, position.Y, size.X, size.Y);
        }
        public static AABB Create(Rectangle rectangle)
        {
            return Create(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static AABB CreateCentered(Vector2 position, Vector2 size)
        {
            return CreateCentered(position.X, position.Y, size.X, size.Y);
        }

        public static AABB CreateBoundingBox(params AABB[] bounds)
        {
            if (bounds.Length < 0) throw new ArgumentException("Need at least 1 bounds to succeed");

            return new AABB()
            {
                Left = bounds.Min(x => x.Left),
                Right = bounds.Max(x => x.Right),
                Top = bounds.Min(x => x.Top),
                Bottom = bounds.Max(x => x.Bottom)
            };
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Left, Right, Top, Bottom);
        }
    }
}
