using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Input;
using PinguinGame.Pinguins.Levels;
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

        private List<Penguin> _penguins;
        public IEnumerable<Penguin> Penguins => _penguins;
        public IEnumerable<PlayerInfo> Players => Fight.Players;
        public Fight Fight { get; private set; }

        public Level Level { get; private set; }
        public Camera LevelCamera { get; private set; }
        public LevelGraphics LevelGraphics { get; private set; }

        public PenguinWorld(GraphicsDevice device, Level level, PlayerInfo[] players, InputService inputService) // Probably should have a levelservice or something
        {
            _penguins = new List<Penguin>();
            InputService = inputService;

            Fight = new Fight(players);

            Level = level;
            LevelCamera = new Camera(256, 1);
            LevelGraphics = new LevelGraphics(device, 256, 256, new LevelGraphicsSettings());
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
                var block = Level.GetIceBlockForPosition(p.Position);
                
                if(block == null || !block.Solid)
                {
                    p.Drown();
                    result.Add(p);
                }
            }

            return result;
        }

        public void UpdatePenguinsWithInput(float delta, Func<Penguin, PenguinInput> inputCreator)
        {
            UpdateLevel(delta);
            foreach (var (penguin, input) in _penguins.Select(x => (x, inputCreator(x))))
            {
                penguin.Update(input, delta);
            }
        }

        public void UpdateLevel(float delta)
        {
            foreach (var block in Level.Blocks)
            {
                block.Update(delta);
            }

            if ((new Random()).NextDouble() < 0.005f){
                var avail = Level.Blocks.Where(x => x.State is IceBlockIdleState && x.Sinkable);
                if(avail.Count() > 0)
                {
                    var block = avail.Random();
                    block.State = new IceBlockSinkingState(block);
                }
            }
        }
        
        public void DrawWorld(Graphics2D graphics)
        {
            LevelGraphics.Draw(LevelCamera, Level);

            graphics.Clear(LevelGraphics.Settings.WaterColor);
            
            graphics.DrawTexture(LevelGraphics.RenderTarget, new Vector2(-128, -128), new Vector2(256, 256));

            var penguins = _penguins.OrderBy(x => x.Physics.Position.Y);

            foreach (var penguin in penguins)
            {
                var block = Level.GetIceBlockForPosition(penguin.Position);

                float height = 0;

                if(block != null)
                {
                    height = Math.Max(0, block.Height);
                }

                penguin.Draw(graphics, penguin.Graphics, height);
            }

        }
    }
}
