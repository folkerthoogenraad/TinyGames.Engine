using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.GameStates
{
    public interface IGameState
    {
        public void Init();
        public void Update(float delta);
        public void Draw(Graphics2D graphics);
        public void Destroy();
    }
}
