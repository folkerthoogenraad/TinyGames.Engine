using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace PinguinGame.MiniGames.Generic
{
    public static class CharacterGraphicsExtension
    {
        public static void DrawShadow(this CharacterGraphics self, Graphics2D graphics, Vector2 position, float height)
        {
            graphics.DrawSpriteWithDepths(self.Shadow, position - new Vector2(0, height), 
                GraphicsHelper.YToDepth(position.Y - self.Shadow.Origin.Y), 
                GraphicsHelper.YToDepth(position.Y + self.Shadow.Height - self.Shadow.Origin.Y), Color.White);
        }
        public static void DrawWalk(this CharacterGraphics self, Graphics2D graphics, CharacterGraphics.Facing facing, Vector2 position, float height, float animationTime)
        {
            RawDrawWalk(graphics, self.GetGraphicsForFacing(facing), position, height, animationTime);
        }
        public static void DrawIdle(this CharacterGraphics self, Graphics2D graphics, CharacterGraphics.Facing facing, Vector2 position, float height, float animationTime)
        {
            RawDrawIdle(graphics, self.GetGraphicsForFacing(facing), position, height, animationTime);
        }
        public static void DrawSlide(this CharacterGraphics self, Graphics2D graphics, CharacterGraphics.Facing facing, Vector2 position, float height, float animationTime)
        {
            RawDrawSlide(graphics, self.GetGraphicsForFacing(facing), position, height, animationTime);
        }
        public static void DrawDrown(this CharacterGraphics self, Graphics2D graphics, CharacterGraphics.Facing facing, Vector2 position, float height, float animationTime)
        {
            RawDrawDrown(graphics, self.GetGraphicsForFacing(facing), position, height, animationTime);
        }

        private static void RawDrawWalk(Graphics2D graphics, PenguinGraphicsForFacing facing, Vector2 position, float height, float animationTime)
        {
            graphics.DrawSprite(facing.Walk.GetSpriteForTime(animationTime), position - new Vector2(0, height), 0, GraphicsHelper.YToDepth(position.Y));
        }
        private static void RawDrawSlide(Graphics2D graphics, PenguinGraphicsForFacing facing, Vector2 position, float height, float animationTime)
        {
            graphics.DrawSprite(facing.Slide.GetSpriteForTime(animationTime), position - new Vector2(0, height), 0, GraphicsHelper.YToDepth(position.Y));
        }
        private static void RawDrawIdle(Graphics2D graphics, PenguinGraphicsForFacing facing, Vector2 position, float height, float animationTime)
        {
            graphics.DrawSprite(facing.Idle.GetSpriteForTime(animationTime), position - new Vector2(0, height), 0, GraphicsHelper.YToDepth(position.Y));
        }
        private static void RawDrawDrown(Graphics2D graphics, PenguinGraphicsForFacing facing, Vector2 position, float height, float animationTime)
        {
            graphics.DrawSprite(facing.Drown.GetSpriteForTime(animationTime), position - new Vector2(0, height), 0, GraphicsHelper.YToDepth(position.Y));
        }
    }
}
