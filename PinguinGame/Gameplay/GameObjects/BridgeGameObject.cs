using Microsoft.Xna.Framework;
using PinguinGame.Graphics;
using PinguinGame.Gameplay.Behaviours;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Scenes.Extensions;

namespace PinguinGame.Gameplay.GameObjects
{
    [RequireSceneBehaviour(typeof(IceGameGraphics))]
    public class BridgeGameObject : GameObject, IDrawable2D, IWalkable
    {
        public Vector2 Position;
        public Vector2 Size;

        public float Height { get; set; } = 6;
        public Sprite Sprite { get; set; }

        public BridgeGameObject(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public override void Init()
        {
            base.Init();

            var sprites = Scene.GetIceGameGraphics();
            
            Sprite = sprites.Bridge;
        }

        public void Draw(Graphics2D graphics)
        {
            int spriteX = Sprite.SourceRectangle.X;
            int spriteY = Sprite.SourceRectangle.Y;
            int spriteWidth = Sprite.SourceRectangle.Width;
            int spriteHeight = Sprite.SourceRectangle.Height;

            int xOffset = 0;
            int yOffset = 0;
            int width = (int)Size.X;
            int height = (int)Size.Y;

            while(height - yOffset > 0)
            {
                while(width - xOffset > 0)
                {

                    int w = Math.Min(width, spriteWidth);
                    int h = Math.Min(height, spriteHeight);

                    var pos = Position + new Vector2(xOffset, yOffset);

                    graphics.DrawTextureRegionWithDepths(Sprite.Texture, 
                        new Rectangle(spriteX, spriteY, w, h),
                        pos - new Vector2(0, Height), new Vector2(w, h),
                        GraphicsHelper.YToDepth(pos.Y), 
                        GraphicsHelper.YToDepth(pos.Y + h),
                        Color.White);

                    xOffset += spriteWidth;
                }

                xOffset = 0;
                yOffset += spriteHeight;
            }
        }

        public GroundInfo GetGroundInfo(Vector2 point)
        {
            if (AABB.Create(Position, Size).Contains(point))
            {
                return GroundInfo.Solid(GroundMaterial.Wood, Height);
            }
            return GroundInfo.Empty();
        }
    }
}
