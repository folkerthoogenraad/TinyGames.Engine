using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.MiniGames.Ice;
using PinguinGame.Screens.Resources;
using PinguinGame.Screens.UI;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators;
using TinyGames.Engine.Maths;

namespace PinguinGame.MiniGames.Ice.GameStates
{
    public class CountdownPlayState : GameState
    {
        public float TickTime = 0.8f;
        public float Timer = 3;
        public int Tick = 3;

        public UICountdownScreen _ui;

        public override void Init(IceGame game, GraphicsDevice device, ContentManager content)
        {
            base.Init(game, device, content);

            Timer = TickTime * 3;


            float angle = 0;
            float anglePerPlayer = (MathF.PI * 2) / game.Fight.Players.Length;

            foreach (var player in game.Players)
            {
                Vector2 pos = Tools.AngleVector(angle) * 16;

                var graphics = player.CharacterInfo.Graphics;

                var sound = CharacterSound.CreateCharacterSound(content);

                var penguin = new Character(game, player, graphics, sound, pos + new Vector2(0, 4));
                penguin.Physics = penguin.Physics.SetFacing(-pos);

                World.AddPenguin(penguin);

                angle += anglePerPlayer;
            }

            World.PlaceCharactersOnGround();

            _ui = new UICountdownScreen(new InGameResources(content));
            _ui.UpdateLayout(World.Camera.Bounds);
        }

        public override GameState Update(float delta)
        {
            Timer -= delta;

            _ui.SetCurrentSecond((int)(Timer / TickTime) + 1);
            _ui.Update(delta);

            if (Timer < 0)
            {
                return new PlayingGameState();
            }

            return this;
        }

        public override void Draw(Graphics2D graphics)
        {
            base.Draw(graphics);

            _ui.Draw(graphics);
        }
    }
}
