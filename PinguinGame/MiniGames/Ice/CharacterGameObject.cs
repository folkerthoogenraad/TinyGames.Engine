using Microsoft.Xna.Framework;
using PinguinGame.MiniGames.Generic;
using PinguinGame.MiniGames.Ice.CharacterStates;
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

namespace PinguinGame.MiniGames.Ice
{
    [RequireSceneBehaviour(typeof(SceneGraphics))]
    [RequireSceneBehaviour(typeof(Walkables))]
    [RequireSceneBehaviour(typeof(IceGameUIGraphics))]
    public class CharacterGameObject : GameObject, IDrawable2D
    {
        private CharacterState _state { get; set; }
        private CharacterState State
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
        public bool CanCollide => !Invunerable && !IsDrowning;

        public bool Invunerable => InvunerableTime > 0;
        public float InvunerableTime { get; set;  } = 0;

        public bool Grounded { get; set; } = true;
        public float Lifetime { get; set; } = 0;

        private Walkables _walkables;

        public PlayerInfo Player { get; private set; }

        public IceGameUIGraphics UIGraphics { get; set; }
        public CharacterPhysics Physics { get; set; }
        public CharacterSettings Settings { get; private set; }
        public CharacterBounce Bounce { get; private set; }
        public CharacterGraphics Graphics { get; private set; }
        public CharacterSnowballGathering SnowballGathering { get; private set; }
        public CharacterSoundComponent Sound { get; private set; }


        public Vector2 Position
        {
            get => Physics.Position;
            set => Physics.Position = value;
        }
        public float Height => -Bounce.Offset.Y;
        public Vector2 Facing => Physics.Facing;
        public float GroundHeight { get; set; } = 0;


        public CharacterGameObject(PlayerInfo player, Vector2 position)
        {
            Player = player;
            Graphics = player.CharacterInfo.Graphics;

            Sound = new CharacterSoundComponent(player.CharacterInfo.Sound);
            Physics = new CharacterPhysics(position);
            Settings = new CharacterSettings();
            Bounce = new CharacterBounce();
            SnowballGathering = new CharacterSnowballGathering();
            State = new CharacterWalkState();
        }

        public override void Init()
        {
            base.Init();

            _walkables = Scene.GetBehaviour<Walkables>();
            UIGraphics = Scene.GetBehaviour<IceGameUIGraphics>();
        }

        public void Update(CharacterInput input, float delta)
        {
            Lifetime += delta;

            if (Invunerable) InvunerableTime -= delta;

            Bounce.Update(delta);
            Sound.Update(this, delta);
            State = State.Update(this, input, delta);

            PlaceOnGround();
            MoveWithGround(delta);
        }

        public void Draw(Graphics2D graphics)
        {
            if (Invunerable && (InvunerableTime % 0.4f > 0.2f)) return;
            
            if (Grounded)
            {
                Graphics.DrawShadow(graphics, Position, GroundHeight);
            }

            if (SnowballGathering.HasSnowball)
            {
                graphics.DrawSprite(UIGraphics.SnowballIndicator, Position - new Vector2(0, 24 + Bounce.Height), 0, GraphicsHelper.YToDepth(Position.Y));
            }

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

            Grounded = result.Solid;
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

        public override void Destroy()
        {
            Sound.StopAll();
        }
    }
}
