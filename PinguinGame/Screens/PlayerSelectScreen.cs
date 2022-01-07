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
    public class PlayerSelectScreen : Screen
    {
        private readonly PlayerService _playerService;
        private readonly InputService _inputService;
        private readonly IScreenService _screens;

        private Font Font;
        private Font Outline;

        public PlayerSelectScreen(IScreenService screens, PlayerService players, InputService inputService)
        {
            _playerService = players;
            _inputService = inputService;
            _screens = screens;
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
                if (input.ActionPressed)
                {
                    _playerService.GetOrJoinPlayerByInputDevice(input.Device);
                }
            }


            foreach (var input in _inputService.InputStates.Where(x => _playerService.IsPlayerJoinedByInputDevice(x.Device)))
            {
                if (input.StartPressed)
                {
                    _screens.ShowInGameScreen();
                }
            }
        }

        public override void Draw()
        {
            base.Draw();

            Graphics.Clear(Color.MistyRose);

            Graphics.Begin(Camera.GetMatrix());

            Graphics.DrawString(Outline, "Players: " + _playerService.PlayerCount, Camera.Position, Color.Black, FontHAlign.Center);
            Graphics.DrawString(Font, "Players: " + _playerService.PlayerCount, Camera.Position, Color.White, FontHAlign.Center);

            Graphics.End();
        }
    }
}
