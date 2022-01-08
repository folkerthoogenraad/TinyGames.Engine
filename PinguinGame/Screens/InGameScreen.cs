using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Input;
using PinguinGame.Pinguins;
using PinguinGame.Player;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Util;

namespace PinguinGame.Screens
{
    public class InGameScreen : Screen
    {
        private readonly PlayerService _playerService;
        private readonly InputService _inputService;
        private readonly IScreenService _screens;

        private Sprite Background;
        private PenguinGraphics PinguinGraphics;

        private List<Penguin> Penguins;

        private float Timer = 0;

        public InGameScreen(IScreenService screens, PlayerService players, InputService inputService)
        {
            _playerService = players;
            _inputService = inputService;
            _screens = screens;

            Penguins = new List<Penguin>();

            foreach(var player in players.Players)
            {
                Penguins.Add(new Penguin(player));
            }
        }

        public override void Init(GraphicsDevice device, ContentManager content)
        {
            base.Init(device, content);

            Camera.Height = 180;

            Background = new Sprite(content.Load<Texture2D>("Sprites/World")).CenterOrigin();

            PinguinGraphics = new PenguinGraphics(content.Load<Texture2D>("Sprites/PinguinSheet"));
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            Timer += delta;

            foreach(var (penguin, input) in Penguins.Select(x => (x, _inputService.GetInputStateForDevice(x.Player.InputDevice))))
            {
                penguin.Update(new PenguinInput()
                {
                    MoveDirection = input.MoveDirection,
                    SlideStart = input.ActionPressed,
                    SlideHold = input.Action
                }, delta);
            }

            // Bonks!
            foreach(var (a, b) in Penguins.Combinations())
            {
                var p1 = a.Position;
                var p2 = b.Position;

                var dir = p2 - p1;
                var dist = dir.Length();

                if (dist > 8) continue;
                if (dist == 0) continue;

                dir /= dist;

                var totalVelocity = (a.Physics.Velocity - b.Physics.Velocity).Length();
                var bonkVelocity = Math.Max(1, totalVelocity / 2);

                a.Bonk(-dir * bonkVelocity);
                b.Bonk(dir * bonkVelocity);
            }
        }

        public override void Draw()
        {
            base.Draw();

            Graphics.Clear(new Color(125, 170, 207));

            Graphics.Begin(Camera.GetMatrix());

            Graphics.DrawSprite(Background, Vector2.Zero);

            var penguins = Penguins.OrderBy(x => x.Physics.Position.Y);

            foreach (var penguin in penguins)
            {
                penguin.Draw(Graphics, PinguinGraphics);
            }


            Graphics.End();
        }

    }
}
