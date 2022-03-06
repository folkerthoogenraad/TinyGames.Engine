using Microsoft.Xna.Framework;
using PinguinGame.Gameplay.Generic;
using PinguinGame.Gameplay.CharacterStates;
using PinguinGame.Player;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Scenes.Extensions;
using PinguinGame.Graphics;
using PinguinGame.Gameplay.Behaviours;
using PinguinGame.Gameplay.GameObjects.CharacterStates;
using PinguinGame.Gameplay.Components;

namespace PinguinGame.Gameplay.GameObjects
{
    [RequireSceneBehaviour(typeof(GraphicsSceneBehaviour))]
    [RequireSceneBehaviour(typeof(WalkablesSceneBehaviour))]
    [RequireSceneBehaviour(typeof(IceGameUIGraphics))]
    public class CharacterGameObject : GameObject, IDrawable2D
    {
        private CharacterState _state { get; set; }
        public CharacterState State
        {
            get => _state;
            set {
                if (_state != value)
                {
                    _state?.Destroy();
                    _state = value;
                    _state?.Init(this);
                }
            }
        }
        public bool IsBonking => State is CharacterBonkState;
        public bool IsSliding => State is CharacterSlideState;
        public bool IsWalking => State is CharacterWalkState;
        public bool IsDrowning => State is CharacterDrownState;
        public bool IsGathering => State is CharacterGatherSnowState;
        public bool IsInVehicle => State is CharacterVehicleState;

        public bool CanCollide => !Invunerable && !IsDrowning && !IsInVehicle;

        public bool Invunerable => InvunerableTime > 0;
        public float InvunerableTime { get; set;  } = 0;

        public bool Grounded { get; set; } = true;
        public float Lifetime { get; set; } = 0;

        private WalkablesSceneBehaviour _walkables;

        public PlayerInfo Player { get; private set; }

        public IceGameUIGraphics UIGraphics { get; set; }
        public CharacterPhysicsComponent Physics { get; set; }
        public CharacterSettings Settings { get; private set; }
        public HeightComponent Bounce { get; private set; }
        public CharacterGraphics Graphics { get; private set; }
        public CharacterSoundComponent Sound { get; private set; }

        public InventoryComponent Inventory { get; private set; }


        public Vector2 Position
        {
            get => Physics.Position;
            set => Physics.Position = value;
        }
        public float Height => -Bounce.Offset.Y;
        public Vector2 Facing => Physics.Facing.Normalized();
        public float GroundHeight { get; set; } = 0;
        public float HeightFromGround => Height - GroundHeight;


        public CharacterGameObject(PlayerInfo player, Vector2 position)
        {
            Player = player;
            Graphics = player.CharacterInfo.Graphics;

            Settings = new CharacterSettings();

            Bounce = new HeightComponent();
            Physics = new CharacterPhysicsComponent(position);
            Inventory = new InventoryComponent();
            Sound = new CharacterSoundComponent(player.CharacterInfo.Sound);

            // TODO add all components here
            AddComponent(Bounce);
            AddComponent(Physics);
            AddComponent(Inventory);
            AddComponent(Sound);

            State = new CharacterWalkState();
        }

        public override void Init()
        {
            base.Init();

            _walkables = Scene.GetBehaviour<WalkablesSceneBehaviour>();
            UIGraphics = Scene.GetBehaviour<IceGameUIGraphics>();
        }

        public void Update(CharacterInput input, float delta)
        {
            Lifetime += delta;

            if (Invunerable) InvunerableTime -= delta;

            State = State.Update(this, input, delta);

            PlaceOnGround();
            MoveWithGround(delta);
        }

        public void Draw(Graphics2D graphics)
        {
            if (Invunerable && (InvunerableTime % 0.4f > 0.2f)) return;
            
            if (Grounded)
            {
                // TODO fix this, grounded is not grounded 
                Graphics.DrawShadow(graphics, Position, GroundHeight + 0.1f);
            }

            if (Inventory.HasSnowball)
            {
                var pos = Position + Facing * 4;
                graphics.DrawSprite(UIGraphics.SnowballIndicator, pos - new Vector2(0, Height + 6), 0, GraphicsHelper.YToDepth(pos.Y + 2));
            }

            graphics.DrawSpriteFlat(
                UIGraphics.DirectionIndicator, 
                Position,
                new Vector2(1, 1),
                Facing.GetAngleInDegrees(), 
                Height + 1, 
                Player.Color);

            State.Draw(graphics, this, Graphics);
        }
        public void PlaceOnGround()
        {
            var result = _walkables.GetGroundInfo(Position);

            if (IsDrowning)
            {
                GroundHeight = 0;
                Bounce.GroundHeight = GroundHeight;
                return;
            }

            GroundHeight = result.Height;
            Bounce.GroundHeight = GroundHeight;

            Bounce.ClampPosition();

            Grounded = result.IsSolid && HeightFromGround < 1;
        }
        public void MoveWithGround(float delta)
        {
            var result = _walkables.GetGroundInfo(Position);

            Position += result.Velocity * delta;
        }
        public void Bonk(Vector2 velocity, float duration = 1)
        {
            State = new CharacterBonkState(Scene.GetBehaviour<IceGameEffects>(), velocity, duration);
        }
        public void Drown()
        {
            State = new CharacterDrownState();
        }

        public void OnVehicleCollision(ShoppingCartGameObject vehicle)
        {
            if (!vehicle.HasController)
            {
                State = new CharacterVehicleState(vehicle);
            }
        }

        public override void Destroy()
        {
            Sound.StopAll();
        }
    }
}
