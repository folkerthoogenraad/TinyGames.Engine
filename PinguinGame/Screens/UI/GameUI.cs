using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace PinguinGame.Screens.UI
{
    public class GameUI
    {
        public AABB Bounds { get; set; }

        public GameUISkin Skin { get; set; }

        public GameUI(GameUISkin skin)
        {
            Skin = skin;
        }

        public virtual void Update(float delta, AABB bounds)
        {
            Bounds = bounds;
        }

        public virtual void Draw(Graphics2D graphics)
        {

        }
    }
}
