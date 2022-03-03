using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Gameplay;
using PinguinGame.Screens.Resources;
using PinguinGame.Screens.UI;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators;
using TinyGames.Engine.Maths;
using System.Linq;
using TinyGames.Engine.Util;
using PinguinGame.GameStates;
using System.Diagnostics;

namespace PinguinGame.Gameplay.GameStates
{
    public class CountdownPlayState : IceGameState<int>
    {
        public float TickDuration = 0.8f;
        public float Timer = 3;
        public int Tick = 3;

        public UICountdownScreen _ui;

        public CountdownPlayState(IceGame game) : base(game) { }

        public override void Init()
        {
            base.Init();

            Timer = TickDuration * 3;

            _ui = new UICountdownScreen(new InGameResources(Content));
            _ui.UpdateLayout(Game.Camera.Bounds);
        }

        public override void Update(float delta)
        {
            int oldTick = GetWholeTick(Timer) + 1;

            Timer -= delta;

            if (Timer < -TickDuration)
            {
                Complete(0);
                return;
            }

            int newTick = GetWholeTick(Timer) + 1;



            _ui.SetCurrentSecond(newTick);
            _ui.Update(delta);


            if (oldTick != newTick)
            {
                if(newTick == 0)
                {
                    Game.UISoundService.PlayCountdownHigh();
                }
                else
                {
                    Game.UISoundService.PlayCountdownLow();
                }
            }

        }

        public override void Draw(Graphics2D graphics)
        {
            base.Draw(graphics);

            graphics.ClearDepthBuffer();

            _ui.Draw(graphics);
        }

        public int GetWholeTick(float time)
        {
            return (int)Math.Floor(time / TickDuration);
        }
    }
}
