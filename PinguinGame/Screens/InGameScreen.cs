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

namespace PinguinGame.Screens
{
    public class InGameScreen : Screen
    {
        private readonly PlayerService _playerService;
        private readonly InputService _inputService;
        private readonly IScreenService _screens;

        private Sprite Background;
        private PinguinGraphics PinguinGraphics;

        private List<(PlayerInfo Info, PinguinPhysics Physics)> Physics;

        private float Timer = 0;

        public InGameScreen(IScreenService screens, PlayerService players, InputService inputService)
        {
            _playerService = players;
            _inputService = inputService;
            _screens = screens;

            Physics = new List<(PlayerInfo Info, PinguinPhysics Physics)>();

            foreach(var player in players.Players)
            {
                Physics.Add((player, new PinguinPhysics(new PinguinSettings(), Vector2.Zero)));
            }
        }

        public override void Init(GraphicsDevice device, ContentManager content)
        {
            base.Init(device, content);

            Camera.Height = 180;

            Background = new Sprite(content.Load<Texture2D>("Sprites/World")).CenterOrigin();

            PinguinGraphics = new PinguinGraphics(content.Load<Texture2D>("Sprites/PinguinSheet"));
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            Timer += delta;

            Physics = Physics.Select(x => (
                x.Info, 
                x.Physics.Update(delta, _inputService.GetInputStateForDevice(x.Info.InputDevice).MoveDirection)
            )).ToList();
        }

        public override void Draw()
        {
            base.Draw();

            Graphics.Clear(new Color(125, 170, 207));

            Graphics.Begin(Camera.GetMatrix());

            Graphics.DrawSprite(Background, Vector2.Zero);

            var p = Physics.OrderBy(x => x.Physics.Position.Y);

            foreach (var (info, physics) in p)
            {
                PinguinGraphics.DrawShadow(Graphics, physics.Position);
            }

            foreach (var (info, physics) in p)
            {
                var facing = PinguinGraphics.GetFacingFromVector(physics.Facing);

                if (physics.IsWalking)
                {
                    PinguinGraphics.DrawWalk(Graphics, facing, physics.Position, Timer);
                    PinguinGraphics.DrawWalkOverlay(Graphics, facing, physics.Position, Timer, GetColorFromIndex(info.Index));
                }
                else
                {
                    PinguinGraphics.DrawIdle(Graphics, facing, physics.Position, Timer);
                    PinguinGraphics.DrawIdleOverlay(Graphics, facing, physics.Position, Timer, GetColorFromIndex(info.Index));

                }
            }


            Graphics.End();
        }

        private Color GetColorFromIndex(int index)
        {
            if (index == 0) return Color.Red;
            if (index == 1) return Color.Blue;
            if (index == 2) return Color.Yellow;
            if (index == 3) return Color.Green;

            return Color.Black;
        }
    }
}
