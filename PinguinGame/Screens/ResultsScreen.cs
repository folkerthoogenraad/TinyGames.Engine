using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Input;
using PinguinGame.Player;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators;

namespace PinguinGame.Screens
{
    public class ResultsScreen : Screen
    {
        private readonly InputService _inputService;
        private readonly IScreenService _screens;

        public string Text { get; set; }

        private Font Font;
        private Font Outline;

        public ResultsScreen(IScreenService screens, InputService inputService, Fight fight)
        {
            _inputService = inputService;
            _screens = screens;

            var winner = fight.Scoreboard.First().Player;

            Text = $"Player {winner.Index + 1} wins the game!";
        }

        public override void Init(GraphicsDevice device, ContentManager content)
        {
            base.Init(device, content);

            Font = content.LoadFont("Fonts/Font8x10");
            Outline = FontOutline.Create(device, Font);
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            foreach(var input in _inputService.InputStates)
            {
                if (input.StartPressed)
                {
                    _screens.ShowPlayerSelectScreen();
                }
            }
        }

        public override void Draw()
        {
            base.Draw();

            Graphics.Clear(Color.MistyRose);

            Graphics.Begin(Camera.GetMatrix());

            Graphics.DrawString(Outline, Text, Camera.Position, Color.Black, FontHAlign.Center);
            Graphics.DrawString(Font, Text, Camera.Position, Color.White, FontHAlign.Center);

            Graphics.End();
        }
    }
}
