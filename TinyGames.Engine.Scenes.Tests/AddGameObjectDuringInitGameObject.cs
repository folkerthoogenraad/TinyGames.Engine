using System;
using System.Collections.Generic;
using System.Text;
using Tinygames.Engine.Scenes;

namespace TinyGames.Engine.Scenes.Tests
{
    public class AddGameObjectDuringInitGameObject<T> : GameObject where T : GameObject, new()
    {
        public override void Init()
        {
            base.Init();

            Scene.AddGameObject(new T());
        }
    }
}
