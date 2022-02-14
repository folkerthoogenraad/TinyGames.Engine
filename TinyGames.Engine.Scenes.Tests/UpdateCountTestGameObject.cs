using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Scenes;

namespace TinyGames.Engine.Scenes.Tests
{
    public class UpdateCountTestGameObject : GameObject
    {
        public int Count { get; set; }

        public override void Update(float delta)
        {
            base.Update(delta);

            Count++;
        }
    }
}
