using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Player;
using PinguinGame.Screens;
using PinguinGame.Screens.Resources;
using PinguinGame.Screens.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Ice.GameStates
{
    public class KillAudio
    {
        public SoundEffect[] Kill;
        public SoundEffect KillFinal;

        public KillAudio(ContentManager content)
        {
            Kill = new SoundEffect[]
            {
                content.Load<SoundEffect>("SoundEffects/Kills/Kill0"),
                content.Load<SoundEffect>("SoundEffects/Kills/Kill1"),
                content.Load<SoundEffect>("SoundEffects/Kills/Kill2")
            };

            KillFinal = content.Load<SoundEffect>("SoundEffects/Kills/KillFinal");
        }

        public void PlayDeathSound(int playersLeft, bool winning = false)
        {
            if (playersLeft <= 0) return;
            if (playersLeft > Kill.Length) return;

            var sound = Kill[Kill.Length - playersLeft];

            if (winning && playersLeft == 1) 
                KillFinal.Play(0.5f, 0, 0);
            else 
                sound.Play(0.5f, 0, 0);


        }
    }

    public class PlayingGameState : GameState
    {
        private Dictionary<PlayerInfo, int> Lives;
        private List<PlayerInfo> _deathOrder;

        private UIInGame _ui;
        private KillAudio _audio;

        public Dictionary<Character, float> ToRespawn;

        public override void Init(IceGame world, GraphicsDevice device, ContentManager content)
        {
            base.Init(world, device, content);

            _audio = new KillAudio(content);
            _deathOrder = new List<PlayerInfo>();

            Lives = new Dictionary<PlayerInfo, int>();

            foreach (var player in world.Players) Lives.Add(player, 3);

            _ui = new UIInGame(new InGameResources(content), GetUIModel());
            _ui.UpdateLayout(world.Camera.Bounds);
            _ui.FadeIn();

            ToRespawn = new Dictionary<Character, float>();
        }

        public override GameState Update(float delta)
        {
            World.Update(delta);
            World.CharacterCollisions.TryBonkCharacters();
            var results = World.CharacterCollisions.TryDrownCharacters();

            _ui.Update(delta);

            bool encounteredDeath = false;

            foreach(var character in results)
            {
                int lives = Lives[character.Player];
                lives--;
                Lives[character.Player] = lives;

                if(lives == 0)
                {
                    encounteredDeath = true;
                    _deathOrder.Add(character.Player);
                }
                else
                {
                    ToRespawn.Add(character, 1);
                }
            }

            if(results.Any())
            {
                if (encounteredDeath)
                {
                    _audio.PlayDeathSound(
                        Lives.Count(p => p.Value > 0), 
                        Lives.Any(p => p.Value > 0 && World.Fight.IsPlayerWinning(p.Key)));
                }
                _ui.SetModel(GetUIModel());
            }

            foreach(var character in ToRespawn.Keys.ToArray())
            {
                float timer = ToRespawn[character];
                timer -= delta;
                ToRespawn[character] = timer;

                if(timer < 0)
                {
                    ToRespawn.Remove(character);
                    World.RemoveCharacter(character);
                    var player = World.SpawnCharacter(World.FindRandomSpawnPoint(), character.Player);
                    player.InvunerableTime = 2f;
                }
            }

            // When deaths and stuff
            if(_deathOrder.Count > World.Fight.Players.Length - 2)
            {
                var winningPlayer = Lives.Where(x => x.Value > 0).Select(x => x.Key).FirstOrDefault();

                var enumerable = _deathOrder.Reverse<PlayerInfo>();

                if(winningPlayer != null)
                {
                    enumerable = enumerable.Prepend(winningPlayer);
                }

                var roundResults = new RoundResults(enumerable.ToArray());

                World.Fight.AddRound(roundResults);

                return new PostGameState(roundResults);
            }

            return this;
        }

        public override void Draw(Graphics2D graphics)
        {
            base.Draw(graphics);

            World.DrawPlayerIndicators(graphics);

            graphics.ClearDepthBuffer();

            // Draw ui :)

            _ui.Draw(graphics);
        }

        private UIInGameModel GetUIModel()
        {
            return new UIInGameModel()
            {
                Characters = World.Players.Select(player => new UICharacterLivesModel()
                {
                    MaxLives = 3,
                    Lives = Lives[player],
                    Color = player.Color,
                    Icon = player.CharacterInfo.Icon
                }).ToArray()
            };
        }
    }
}
