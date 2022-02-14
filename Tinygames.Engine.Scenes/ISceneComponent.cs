using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Scenes
{
    public interface ISceneComponent
    {
        public void Init(Scene scene);

        public void BeforeUpdate(float delta);
        public void AfterUpdate(float delta);
        
        public void Destroy();
    }
}
