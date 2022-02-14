using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Scenes;

namespace TinyGames.Engine.Scenes.Tests
{
    public class AddGameObjectDuringUpdateGameObject<T> : GameObject where T : GameObject, new()
    {
        private bool _first = true;

        public override void Update(float delta)
        {
            base.Update(delta);

            if (_first)
            {
                _first = false;
                Scene.AddGameObject(new T());
            }
        }
    }
}
