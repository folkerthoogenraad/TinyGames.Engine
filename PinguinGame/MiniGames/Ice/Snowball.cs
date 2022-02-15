using Microsoft.Xna.Framework;
using PinguinGame.Graphics;
using PinguinGame.Player;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Scenes.Extensions;

namespace PinguinGame.MiniGames.Ice
{
    [RequireSceneBehaviour(typeof(SceneGraphics))]
    [RequireSceneBehaviour(typeof(Walkables))]
    [RequireSceneBehaviour(typeof(SnowballGraphics))]
    public class Snowball : GameObject, IDrawable2D
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Lifetime { get; set; }

        public float Angle => 0;
        public float Height { get; set; }
        public bool Collided { get; set; } = false;
        public PlayerInfo Info { get; set; }
        public SnowballGraphics Graphics { get; set; }
        public Walkables Walkables { get; set; }

        public Snowball(PlayerInfo info)
        {
            Info = info;
        }

        public override void Init()
        {
            base.Init();

            Walkables = Scene.GetBehaviour<Walkables>();
            Graphics = Scene.GetBehaviour<SnowballGraphics>();

            var sceneGraphics = Scene.GetBehaviour<SceneGraphics>();
            sceneGraphics.AddDrawable(this);
        }

        public override void Destroy()
        {
            base.Destroy();
            var sceneGraphics = Scene.GetBehaviour<SceneGraphics>();
            sceneGraphics.RemoveDrawable(this);
        }

        public override void Update(float delta)
        {
            if (Collided)
            {
                Scene.RemoveGameObject(this);
                return;
            }

            Position += Velocity * delta;

            Lifetime -= delta;

            if(Lifetime < 0)
            {
                Scene.RemoveGameObject(this);
            }
        }

        public void Draw(Graphics2D graphics)
        {
            var info = Walkables.GetGroundInfo(Position);
            
            graphics.DrawSprite(Graphics.Shadow, Position - new Vector2(0, info.Height), Angle, GraphicsHelper.YToDepth(Position.Y));
            graphics.DrawSprite(Graphics.Sprite, Position - new Vector2(0, Height), Angle, GraphicsHelper.YToDepth(Position.Y));
        }
    }
}
