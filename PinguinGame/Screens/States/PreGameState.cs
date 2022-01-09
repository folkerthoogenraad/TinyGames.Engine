using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Pinguins;
using PinguinGame.Screens.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Graphics;

namespace PinguinGame.Screens.States
{
    public class PreGameState : GameState
    {
        public float Timer = 0;

        private PreGameUI _ui;

        public override void Init(PenguinWorld world, GraphicsDevice device, ContentManager content, GameUISkin skin)
        {
            base.Init(world, device, content, skin);

            _ui = new PreGameUI(Skin, new PreGameUIModel() {
                Scores = world.Fight.Scores,
                Colors = world.Fight.Players.Select(x => x.Color).ToArray(),
            });
            _ui.Update(0, World.Camera.Bounds);
        }

        public override GameState Update(float delta)
        {
            Timer += delta;

            if(Timer > 1.5f)
            {
                return new CountdownPlayState();
            }

            _ui.Update(delta, World.Camera.Bounds);

            return this;
        }

        public override void Draw(Graphics2D graphics)
        {
            base.Draw(graphics);

            _ui.Draw(graphics);
        }
    }
}
