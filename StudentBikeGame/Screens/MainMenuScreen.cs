using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentBikeGame.Screens
{
    public class MainMenuScreen : Screen
    {
        public override void Update(float delta)
        {
            base.Update(delta);
        }

        public override void Draw()
        {
            base.Draw();

            Graphics.Clear(Color.DarkGray);
            Graphics.Begin(Camera.GetMatrix());

            Graphics.End();
        }
    }
}
