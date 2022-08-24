using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Scenes.Extensions
{
    public static class SceneExtensions
    {
        public static void RemoveAll(this Scene scene, Func<GameObject, bool> predecate)
        {
            foreach(var g in scene.GameObjects)
            {
                if (predecate(g))
                {
                    scene.RemoveGameObject(g);
                }
            }
        }
    }
}
