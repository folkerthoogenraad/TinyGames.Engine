﻿using Microsoft.Xna.Framework;
using PinguinGame.Graphics;
using PinguinGame.Gameplay.Behaviours;
using PinguinGame.Player;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Scenes.Extensions;

namespace PinguinGame.Gameplay.GameObjects
{
    [RequireSceneBehaviour(typeof(GraphicsSceneBehaviour))]
    [RequireSceneBehaviour(typeof(WalkablesSceneBehaviour))]
    [RequireSceneBehaviour(typeof(IceGameGraphics))]
    public class SnowballGameObject : GameObject, IDrawable2D
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Lifetime { get; set; }

        public float Angle => 0;
        public float Height { get; set; }
        public bool Collided { get; set; } = false;
        public PlayerInfo Info { get; set; }
        public IceGameGraphics Graphics { get; set; }
        public WalkablesSceneBehaviour Walkables { get; set; }

        public SnowballGameObject(PlayerInfo info)
        {
            Info = info;
        }

        public override void Init()
        {
            base.Init();

            Walkables = Scene.GetBehaviour<WalkablesSceneBehaviour>();
            Graphics = Scene.GetBehaviour<IceGameGraphics>();
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
            
            graphics.DrawSprite(Graphics.SnowballShadow, Position - new Vector2(0, info.Height), Angle, GraphicsHelper.YToDepth(Position.Y));
            graphics.DrawSprite(Graphics.Snowball, Position - new Vector2(0, Height), Angle, GraphicsHelper.YToDepth(Position.Y));
        }
    }
}