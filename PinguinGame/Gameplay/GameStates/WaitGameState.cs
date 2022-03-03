using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Screens.Resources;
using PinguinGame.Screens.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Graphics;

namespace PinguinGame.Gameplay.GameStates
{
    public class WaitGameStateSettings
    {
        public float Time = 1;
        public bool ShouldUpdateWorld = false;
    }

    public class WaitGameState : IceGameState<int>
    {
        public WaitGameStateSettings Settings { get; }

        private float _timer = 0;


        public WaitGameState(IceGame game, WaitGameStateSettings settings) : base(game)
        {
            Settings = settings;
            _timer = settings.Time;
        }

        public override void Update(float delta)
        {
            _timer -= delta;

            if (Settings.ShouldUpdateWorld)
            {
                Game.Update(delta);

                Game.CharacterCollisions.TryBonkCharacters();
                Game.CharacterCollisions.TryDrownCharacters();
            }

            if (_timer <= 0)
            {
                Complete(0);
                return;
            }
        }

        public override void Draw(Graphics2D graphics)
        {
            base.Draw(graphics);
        }
    }
}
