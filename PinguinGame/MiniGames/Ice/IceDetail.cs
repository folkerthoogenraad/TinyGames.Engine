using Microsoft.Xna.Framework;
using PinguinGame.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Scenes.Extensions;

namespace PinguinGame.MiniGames.Ice
{
    [RequireSceneBehaviour(typeof(IceGameGraphics))]
    [RequireSceneBehaviour(typeof(Walkables))]
    public abstract class IceDetail : GameObject, IDrawable2D
    {
        public Vector2 Position { get; set; }
        public float Height { get; set; } = 0;

        public Sprite Sprite { get; set; }
        public Sprite Shadow { get; set; }

        public Walkables Walkables { get; set; }

        public float Angle { get; set; }

        public IceDetail(Vector2 position)
        {
            Position = position;
        }

        public override void Init()
        {
            base.Init();

            Walkables = Scene.GetWalkables();
            Height = Walkables.GetGroundInfo(Position).Height;

            LoadSprites(Scene.GetIceGameGraphics());
        }

        public override void Update(float delta)
        {
            var info = Walkables.GetGroundInfo(Position);

            Height = info.Height;
            Position += info.Velocity * delta;

            Angle = Tools.Lerp(Angle, -info.Velocity.X, delta * 5);
        }

        public void Draw(Graphics2D graphics)
        {
            graphics.DrawSpriteUpright(Sprite, Position, Height, Angle, Color.White);

            if(Shadow != null)
            {
                graphics.DrawSpriteFlat(Shadow, Position, Height + 0.1f, new Color(Color.White, 0.5f));
            }
        }

        public abstract void LoadSprites(IceGameGraphics graphics);
    }
}
