using System;
using System.Collections.Generic;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Diagnostics;

namespace TinyGames.Engine.Scenes.Extensions
{
    public class GraphicsSceneBehaviour : ISceneBehaviour
    {
        public Scene Scene { get; set; }
        public Camera Camera { get; set; }

        public Graphics2D Graphics { get; set; }

        private List<IDrawable2D> _drawables;

        private IGraphicsService _graphicsService;
        public Color BackgroundColor { get; set; }

        public GraphicsSceneBehaviour(IGraphicsService graphics)
        {
            _graphicsService = graphics;
            _drawables = new List<IDrawable2D>();

            Camera = new Camera(_graphicsService.Height, _graphicsService.AspectRatio); // TODO this camera information should come from the scene somehow
            Graphics = new Graphics2D(_graphicsService.Device);

            BackgroundColor = Color.White;
        }

        public void Init(Scene scene)
        {
            Scene = scene;
        }

        public void Destroy()
        {
            Graphics.Dispose();
        }

        public void AfterUpdate(float delta)
        {
            // Do nothing
        }

        public void BeforeUpdate(float delta)
        {
            // Do nothing
        }

        public void Draw()
        {
            Graphics.Begin(Camera.GetProjectionMatrix(), Camera.GetModelMatrix());
            Graphics.Clear(BackgroundColor);

            _drawables = _drawables.OrderBy(x => x.LayerIndex).ToList();

            foreach (var drawable in _drawables) drawable.Draw(Graphics);
            
            Graphics.End();
        }

        public void AddDrawable(IDrawable2D drawable)
        {
            Debug.Assert(!_drawables.Contains(drawable));
            _drawables.Add(drawable);
        }
        public void RemoveDrawable(IDrawable2D drawable)
        {
            Debug.Assert(_drawables.Contains(drawable));
            _drawables.Remove(drawable);
        }
    }

    public static class SceneGraphicsExtensions
    {
        public static GraphicsSceneBehaviour GetSceneGraphics(this Scene scene)
        {
            return scene.GetBehaviour<GraphicsSceneBehaviour>();
        }
        public static Camera GetCamera(this Scene scene)
        {
            return scene.GetSceneGraphics().Camera;
        }
    }
}
