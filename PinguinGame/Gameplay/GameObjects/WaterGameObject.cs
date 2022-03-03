using Microsoft.Xna.Framework;
using PinguinGame.Graphics;
using PinguinGame.Gameplay.Behaviours;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Scenes.Extensions;

namespace PinguinGame.Gameplay.GameObjects
{
    [RequireSceneBehaviour(typeof(GraphicsSceneBehaviour))]
    public class WaterGameObject : GameObject, IDrawable2D, IWalkable
    {
        public Camera Camera { get; set; }
        public IceLevelGraphicsSettings GraphicsSettings { get; set; }

        public WaterGameObject()
        {
            GraphicsSettings = new IceLevelGraphicsSettings();
        }

        public override void Init()
        {
            base.Init();

            var graphics = Scene.GetBehaviour<GraphicsSceneBehaviour>();
            Camera = graphics.Camera;
        }

        public void Draw(Graphics2D graphics)
        {
            graphics.SetBlendMode(BlendMode.Multiply);
            graphics.DrawRectangleFlat(Camera.Bounds.Translated(new Vector2(0, -32)), -32, GraphicsSettings.WaterColor);
            graphics.DrawRectangleFlat(Camera.Bounds, 0, GraphicsSettings.WaterColor);
            graphics.SetBlendMode(BlendMode.Normal);
        }

        public GroundInfo GetGroundInfo(Vector2 point)
        {
            return GroundInfo.Solid(GroundMaterial.Water);
        }
    }
}
