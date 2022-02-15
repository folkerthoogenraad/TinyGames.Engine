using System;
using System.Collections.Generic;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Graphics;

namespace TinyGames.Engine.Scenes.Extensions
{
    public class SceneGraphics : ISceneBehaviour
    {
        private List<IDrawable2D> _drawables;

        public SceneGraphics()
        {
            _drawables = new List<IDrawable2D>();
        }

        public void Init(Scene scene)
        {
            // Do nothing
        }

        public void Destroy()
        {
            // Do nothing
        }

        public void AfterUpdate(float delta)
        {
            // Do nothing
        }

        public void BeforeUpdate(float delta)
        {
            // Do nothing
        }

        public void Draw(Graphics2D graphics)
        {
            foreach (var drawable in _drawables) drawable.Draw(graphics);
        }

        public void AddDrawable(IDrawable2D drawable)
        {
            _drawables.Add(drawable);
        }
        public void RemoveDrawable(IDrawable2D drawable)
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
