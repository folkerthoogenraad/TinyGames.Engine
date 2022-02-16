using System;
using System.Collections.Generic;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Graphics;

namespace TinyGames.Engine.Scenes.Extensions
{
    public class SceneGraphics : ISceneBehaviour
    {
        public Scene Scene { get; set; }
        private List<IDrawable2D> _drawables;

        public SceneGraphics()
        {
            _drawables = new List<IDrawable2D>();
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

        public void Draw(Graphics2D graphics)
        {
            foreach (var drawable in _drawables) drawable.Draw(graphics);
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
        public static SceneGraphics GetSceneGraphics(this Scene scene)
        {
            return scene.GetBehaviour<SceneGraphics>();
        }
    }
}
