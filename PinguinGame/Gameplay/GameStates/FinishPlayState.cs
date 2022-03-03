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

namespace PinguinGame.Gameplay.GameStates
{
    public class FinishPlayState : IceGameState<int>
    {
        public float Timer = 2;

        public UIFinishScreen _ui;

        public FinishPlayState(IceGame game) : base(game) { }

        public override void Init()
        {
            base.Init();

            _ui = new UIFinishScreen(new InGameResources(Content));
            _ui.UpdateLayout(Game.Camera.Bounds);
        }

        public override void Update(float delta)
        {
            Timer -= delta;

            _ui.Update(delta);

            Game.Update(delta * 0.5f);

            Game.CharacterCollisions.TryBonkCharacters();
            Game.CharacterCollisions.TryDrownCharacters();

            if (Timer < 0)
            {
                Complete(0);
            }
        }

        public override void Draw(Graphics2D graphics)
        {
            base.Draw(graphics);

            graphics.ClearDepthBuffer();

            _ui.Draw(graphics);
        }
    }
}
