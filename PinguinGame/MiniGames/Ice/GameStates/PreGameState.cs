using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Screens.Resources;
using PinguinGame.Screens.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Ice.GameStates
{
    public class PreGameState : GameState
    {
        public float Timer = 0;

        //private UIPreGame _ui;

        public override void Init(IceGame world, GraphicsDevice device, ContentManager content)
        {
            base.Init(world, device, content);

            //_ui = new UIPreGame(new InGameResources(content), new PreGameUIModel() {
            //    Scores = world.Fight.Scores,
            //    Colors = world.Fight.Players.Select(x => x.Color).ToArray(),
            //});
            //_ui.UpdateLayout(World.Camera.Bounds);
        }

        public override GameState Update(float delta)
        {
            Timer += delta;

            if(Timer > 1.5f)
            {
                return new CountdownPlayState();
            }

            //_ui.Update(delta);

            return this;
        }

        public override void Draw(Graphics2D graphics)
        {
            base.Draw(graphics);

            //graphics.ClearDepthBuffer();

            //_ui.Draw(graphics);
        }
    }
}
