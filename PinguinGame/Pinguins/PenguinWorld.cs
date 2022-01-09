using Microsoft.Xna.Framework;
using PinguinGame.Input;
using PinguinGame.Player;
using PinguinGame.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Util;

namespace PinguinGame.Pinguins
{
    public class PenguinWorld
    {
        public Camera Camera { get; set; }
        public InputService InputService { get; private set; }

        private Sprite Background;

        private List<Penguin> _penguins;
        public IEnumerable<Penguin> Penguins => _penguins;
        public IEnumerable<PlayerInfo> Players => Fight.Players;
        public Fight Fight { get; private set; }


        public PenguinWorld(Sprite background, PlayerInfo[] players, InputService inputService)
        {
            Background = background;
            _penguins = new List<Penguin>();
            InputService = inputService;

            Fight = new Fight(players);
        }

        public void AddPenguin(Penguin penguin)
        {
            _penguins.Add(penguin);
        }

        public void RemovePenguin(Penguin penguin)
        {
            _penguins.Remove(penguin);
        }

        public void RemoveAllPenguins()
        {
            _penguins.Clear();
        }

        public void BonkPenguins()
        {
            // Bonks!
            foreach (var (a, b) in _penguins.Combinations())
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

        public List<Penguin> DrownPenguins()
        {
            var result = new List<Penguin>();

            foreach(var p in _penguins.Where(x => !x.IsDrowning))
            {
                var offset = p.Position - Vector2.Zero;
                var dist = offset.Length();

                if(dist > 48)
                {
                    p.Drown();

                    result.Add(p);
                }
            }

            return result;
        }

        public void UpdatePenguinsWithInput(float delta, Func<Penguin, PenguinInput> inputCreator)
        {
            foreach (var (penguin, input) in _penguins.Select(x => (x, inputCreator(x))))
            {
                penguin.Update(input, delta);
            }
        }
        
        public void DrawWorld(Graphics2D graphics)
        {
            graphics.DrawSprite(Background, Vector2.Zero);

            var penguins = _penguins.OrderBy(x => x.Physics.Position.Y);

            foreach (var penguin in penguins)
            {
                penguin.Draw(graphics, penguin.Graphics);
            }

        }
    }
}
