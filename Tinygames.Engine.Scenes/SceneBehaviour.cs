using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Scenes
{
    public class SceneBehaviour : ISceneBehaviour
    {
        public Scene Scene { get; set; }
        public virtual void Init(Scene scene) { Scene = scene; }
        public virtual void BeforeUpdate(float delta) { }
        public virtual void AfterUpdate(float delta) { }
        public virtual void Destroy() { }
    }
}
