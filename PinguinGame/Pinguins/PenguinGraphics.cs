using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace PinguinGame.Pinguins
{
    internal class PenguinGraphicsForFacing
    {
        public Animation Idle;
        public Animation IdleOverlay;
        public Animation Walk;
        public Animation WalkOverlay;
        public Animation Slide;
        public Animation SlideOverlay;
        public Animation Drown;
        public Animation DrownOverlay;
    }

    public class PenguinGraphics
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

        public PenguinGraphics(Texture2D texture)
        {
            _graphics = new Dictionary<Facing, PenguinGraphicsForFacing>();

            _graphics.Add(Facing.Down, GetGraphicsWithOffset(texture, 0));
            _graphics.Add(Facing.Right, GetGraphicsWithOffset(texture, 16));
            _graphics.Add(Facing.Up, GetGraphicsWithOffset(texture, 32));
            _graphics.Add(Facing.Left, GetGraphicsWithOffset(texture, 48));

            _shadow = new Sprite(texture, new Rectangle(112, 112, 16, 16)).CenterOrigin();
        }

        public void DrawShadow(Graphics2D graphics, Vector2 position)
        {
            graphics.DrawSprite(_shadow, position);
        }
        public void DrawWalk(Graphics2D graphics, Facing facing, Vector2 position, float animationTime)
        {
            RawDrawWalk(graphics, _graphics[facing], position, animationTime);
        }
        public void DrawWalkOverlay(Graphics2D graphics, Facing facing, Vector2 position, float animationTime, Color color)
        {
            RawDrawWalkOverlay(graphics, _graphics[facing], position, animationTime, color);
        }
        public void DrawIdle(Graphics2D graphics, Facing facing, Vector2 position, float animationTime)
        {
            RawDrawIdle(graphics, _graphics[facing], position, animationTime);
        }
        public void DrawIdleOverlay(Graphics2D graphics, Facing facing, Vector2 position, float animationTime, Color color)
        {
            RawDrawIdleOverlay(graphics, _graphics[facing], position, animationTime, color);
        }

        public void DrawSlide(Graphics2D graphics, Facing facing, Vector2 position, float animationTime)
        {
            RawDrawSlide(graphics, _graphics[facing], position, animationTime);
        }
        public void DrawSlideOverlay(Graphics2D graphics, Facing facing, Vector2 position, float animationTime, Color color)
        {
            RawDrawSlideOverlay(graphics, _graphics[facing], position, animationTime, color);
        }
        public void DrawDrown(Graphics2D graphics, Facing facing, Vector2 position, float animationTime)
        {
            RawDrawDrown(graphics, _graphics[facing], position, animationTime);
        }
        public void DrawDrownOverlay(Graphics2D graphics, Facing facing, Vector2 position, float animationTime, Color color)
        {
            RawDrawDrownOverlay(graphics, _graphics[facing], position, animationTime, color);
        }

        private void RawDrawWalk(Graphics2D graphics, PenguinGraphicsForFacing facing, Vector2 position, float animationTime)
        {
            graphics.DrawSprite(facing.Walk.GetSpriteForTime(animationTime), position);
        }
        private void RawDrawSlide(Graphics2D graphics, PenguinGraphicsForFacing facing, Vector2 position, float animationTime)
        {
            graphics.DrawSprite(facing.Slide.GetSpriteForTime(animationTime), position);
        }
        private void RawDrawIdle(Graphics2D graphics, PenguinGraphicsForFacing facing, Vector2 position, float animationTime)
        {
            graphics.DrawSprite(facing.Idle.GetSpriteForTime(animationTime), position);
        }
        private void RawDrawDrown(Graphics2D graphics, PenguinGraphicsForFacing facing, Vector2 position, float animationTime)
        {
            graphics.DrawSprite(facing.Drown.GetSpriteForTime(animationTime), position);
        }

        private void RawDrawSlideOverlay(Graphics2D graphics, PenguinGraphicsForFacing facing, Vector2 position, float animationTime, Color color)
        {
            graphics.DrawSprite(facing.SlideOverlay.GetSpriteForTime(animationTime), position, 0, 0, color);
        }
        private void RawDrawWalkOverlay(Graphics2D graphics, PenguinGraphicsForFacing facing, Vector2 position, float animationTime, Color color)
        {
            graphics.DrawSprite(facing.WalkOverlay.GetSpriteForTime(animationTime), position, 0, 0, color);
        }
        private void RawDrawIdleOverlay(Graphics2D graphics, PenguinGraphicsForFacing facing, Vector2 position, float animationTime, Color color)
        {
            graphics.DrawSprite(facing.IdleOverlay.GetSpriteForTime(animationTime), position, 0, 0, color);
        }
        private void RawDrawDrownOverlay(Graphics2D graphics, PenguinGraphicsForFacing facing, Vector2 position, float animationTime, Color color)
        {
            graphics.DrawSprite(facing.DrownOverlay.GetSpriteForTime(animationTime), position, 0, 0, color);
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

                // Overlays
                IdleOverlay = Animation.FromSprites(6,
                    new Sprite(texture, new Rectangle(xoffset + 64, 0, 16, 16)).SetOrigin(8, 16)
                    ),

                WalkOverlay = Animation.FromSprites(6,
                    new Sprite(texture, new Rectangle(xoffset + 64, 16, 16, 16)).SetOrigin(8, 16),
                    new Sprite(texture, new Rectangle(xoffset + 64, 32, 16, 16)).SetOrigin(8, 16)
                    ),

                SlideOverlay = Animation.FromSprites(6,
                    new Sprite(texture, new Rectangle(xoffset + 64, 48, 16, 16)).SetOrigin(8, 16),
                    new Sprite(texture, new Rectangle(xoffset + 64, 64, 16, 16)).SetOrigin(8, 16)
                    ),
                DrownOverlay = Animation.FromSprites(3,
                    new Sprite(texture, new Rectangle(xoffset + 64, 80, 16, 16)).SetOrigin(8, 16),
                    new Sprite(texture, new Rectangle(xoffset + 64, 96, 16, 16)).SetOrigin(8, 16)
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
    }
}
