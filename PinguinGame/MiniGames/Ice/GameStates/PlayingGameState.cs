using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Player;
using PinguinGame.Screens.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PinguinGame.MiniGames.Ice.GameStates
{
    public class PlayingGameState : GameState
    {
        private List<PlayerInfo> _deathOrder;

        public override void Init(IceWorld world, GraphicsDevice device, ContentManager content, GameUISkin skin)
        {
            base.Init(world, device, content, skin);

            _deathOrder = new List<PlayerInfo>();
        }

        public override GameState Update(float delta)
        {
            World.UpdatePenguinsWithInput(delta, x => {
                var raw = World.InputService.GetInputStateForDevice(x.Player.InputDevice);

                return new PenguinInput()
                {
                    MoveDirection = raw.MoveDirection,
                    SlideStart = raw.ActionPressed,
                    SlideHold = raw.Action
                };
            });

            World.BonkPenguins();

            var results = World.DrownPenguins(delta);

            _deathOrder.AddRange(results.Select(x => x.Player));

            if(_deathOrder.Count > World.Fight.Players.Length - 2)
            {
                var winningPlayer = World.Penguins.Where(x => !x.IsDrowning).FirstOrDefault();

                var enumerable = _deathOrder.Reverse<PlayerInfo>();

                if(winningPlayer != null)
                {
                    enumerable = enumerable.Prepend(winningPlayer.Player);
                }

                var roundResults = new RoundResults(enumerable.ToArray());


                return new PostGameState(roundResults);
            }

            return this;
        }
    }
}
