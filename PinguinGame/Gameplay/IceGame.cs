using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Audio;
using PinguinGame.Graphics;
using PinguinGame.Input;
using PinguinGame.Gameplay.Generic;
using PinguinGame.Gameplay.Behaviours;
using PinguinGame.Gameplay.CharacterStates;
using PinguinGame.Gameplay.GameObjects;
using PinguinGame.Player;
using PinguinGame.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Collections;
using TinyGames.Engine.Extensions;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Scenes.Extensions;
using TinyGames.Engine.Util;
using PinguinGame.Gameplay.GameObjects.IceBlockStates;

namespace PinguinGame.Gameplay
{
    public class IceGame
    {
        public Camera Camera { get; set; }
        public IEnumerable<CharacterGameObject> Characters => Scene.FindGameObjectsOfType<CharacterGameObject>();
        public PlayerInfo[] Players { get; private set; }

        public IMiniGameInputService<CharacterInput> InputService { get; set; }
        public IUISoundService UISoundService { get; set; }
        public IScreenService ScreenService { get; set; }

        private List<IceBlockBehaviour> _iceBlockBehaviour;

        public ContentManager Content { get; set; }
        
        public Scene Scene { get; set; }
        public GraphicsSceneBehaviour GraphicsBehaviour { get; set; }
        public CharacterCollisionBehaviour CharacterCollisions { get; set; }
        public WalkablesSceneBehaviour Walkables { get; set; }

        public IceGame(IServiceProvider services, LevelInfo level, PlayerInfo[] players, IMiniGameInputService<CharacterInput> inputService, IUISoundService uiSoundService, IScreenService screenservice)
        {
            Players = players;

            Content = new ContentManager(services);
            Content.RootDirectory = "Content";

            UISoundService = uiSoundService;
            ScreenService = screenservice;

            InputService = inputService;


            // TODO abstract this away somewhere
            _iceBlockBehaviour = new List<IceBlockBehaviour>() { 
                new RandomSinkIceBlockBehaviour(),
                new TimedSinkIceBlockBehaviour(),
                new TimedDriftIceBlockBehaviour(),
            };


            Scene = Content.LoadScene(level.File);

            CharacterCollisions = Scene.GetBehaviour <CharacterCollisionBehaviour> ();
            GraphicsBehaviour = Scene.GetBehaviour<GraphicsSceneBehaviour>();
            Walkables = Scene.GetBehaviour<WalkablesSceneBehaviour>();

            Scene.Init();
        }

        public void AddSnowball(SnowballGameObject ball)
        {
            Scene.AddGameObject(ball);
        }

        public void AddCharacter(CharacterGameObject character)
        {
            Scene.AddGameObject(character);
        }

        public void RemoveCharacter(CharacterGameObject character)
        {
            Scene.RemoveGameObject(character);
        }

        public void RemoveAllCharacters()
        {
            foreach(var character in Characters)
            {
                Scene.RemoveGameObject(character);
            }
        }

        public void ResetIceBlocks()
        {
            foreach(var block in Scene.FindGameObjectsOfType<IceBlockGameObject>())
            {
                block.State = new IceBlockIdleState(block);
            }
            _iceBlockBehaviour = new List<IceBlockBehaviour>() {
                new RandomSinkIceBlockBehaviour(),
                new TimedSinkIceBlockBehaviour(),
                new TimedDriftIceBlockBehaviour(),
            };
            Scene.Sync();
        }

        // Extension methods maybe?
        public Vector2 FindApplicableSpawnLocation()
        {
            return Scene.FindGameObjectsOfType<SpawnGameObject>().Select(x => x.Position).Where(spawn => {
                var block = Walkables.GetGroundInfo(spawn);

                if (!block.IsSolid) return false;

                return true;
            }).RandomOrDefault();
        }

        public Vector2 FindRandomSpawnPoint()
        {
            return FindApplicableSpawnLocation();
            // return new Random().NextPointsInPolygon(Level.Blocks.Where(x => x.IsIdle).Random().Polygon, 1).First();
        }

        public CharacterGameObject SpawnCharacter(Vector2 spawnLocation, PlayerInfo player)
        {
            var character = new CharacterGameObject(player, spawnLocation);
            character.Physics = character.Physics.SetFacing(-spawnLocation);

            AddCharacter(character);

            // TODO this is kinda ugly :)
            Scene.Sync();

            return character;
        }

        public void PlaceCharactersOnGround()
        {
            foreach (var penguin in Characters)
            {
                penguin.PlaceOnGround();
            }
        }

        public void Update(float delta)
        {
            UpdateLevel(delta);

            Scene.Update(delta);
            UpdateCharacters(delta);
        }
        public void UpdateLevel(float delta)
        {
            foreach(var behaviour in _iceBlockBehaviour)
            {
                behaviour.Update(Scene.FindGameObjectsOfType<IceBlockGameObject>(), delta);
            }
        }

        public void UpdateCharacters(float delta)
        {
            foreach (var (character, input) in Characters.Select(x => (x, InputService.GetInputForPlayer(x.Player))).ToArray())
            {
                character.Update(input, delta);
            }
        }
        
        public void DrawWorld(Graphics2D graphics)
        {
            graphics.Clear(Color.White);

            GraphicsBehaviour.Draw();
        }


        public void Destroy()
        {
            Scene.Destroy();
            Content.Unload();
        }
    }
}
