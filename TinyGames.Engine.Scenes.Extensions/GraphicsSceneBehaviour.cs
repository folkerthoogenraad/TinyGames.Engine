using System;
using System.Collections.Generic;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Graphics;
using Microsoft.Xna.Framework;

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

            Camera = new Camera(_graphicsService.Height, _graphicsService.AspectRatio);
            Graphics = new Graphics2D(_graphicsService.Device);

            BackgroundColor = Color.White;
        }

        public void Init(Scene scene)
        {
            Scene = scene;

            Scene.OnGameObjectCreated += OnGameObjectCreated;
            Scene.OnGameObjectDestroyed += OnGameObjectDestroyed;
        }

        public void Destroy()
        {
            Scene.OnGameObjectCreated -= OnGameObjectCreated;
            Scene.OnGameObjectDestroyed -= OnGameObjectDestroyed;

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

        public void OnGameObjectCreated(Scene scene, GameObject obj)
        {
            if (obj is IDrawable2D) AddManualDrawable(obj as IDrawable2D);
        }
        public void OnGameObjectDestroyed(Scene scene, GameObject obj)
        {
            if (obj is IDrawable2D) RemoveManualDrawable(obj as IDrawable2D);
        }

        public void Draw()
        {
            Graphics.Begin(Camera.GetMatrix());
            Graphics.Clear(BackgroundColor);

            foreach (var drawable in _drawables) drawable.Draw(Graphics);
            
            Graphics.End();
        }

        public void AddManualDrawable(IDrawable2D drawable)
        {
            _drawables.Add(drawable);
        }
        public void RemoveManualDrawable(IDrawable2D drawable)
        {
            _drawables.Remove(drawable);
        }
    }

    public static class SceneGraphicsExtensions
    {
        public static GraphicsSceneBehaviour GetSceneGraphics(this Scene scene)
        {
            return scene.GetBehaviour<GraphicsSceneBehaviour>();
        }
    }
}
