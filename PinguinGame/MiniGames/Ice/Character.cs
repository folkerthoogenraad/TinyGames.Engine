using Microsoft.Xna.Framework;
using PinguinGame.MiniGames.Generic;
using PinguinGame.MiniGames.Ice.CharacterStates;
using PinguinGame.Player;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Ice
{
    public class Character
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

        public bool Grounded { get; set; } = true;

        public IceGame Level { get; private set; }
        public PlayerInfo Player { get; private set; }
        public CharacterPhysics Physics { get; set; }
        public CharacterSettings Settings { get; private set; }
        public CharacterBounce Bounce { get; private set; } // TODO merge this with the groundheight thing
        public CharacterGraphics Graphics { get; private set; }
        public CharacterSnowballGathering SnowballGathering { get; private set; }
        public CharacterSound Sound { get; private set; }

        public Vector2 Position
        {
            get => Physics.Position;
            set => Physics.Position = value;
        }
        public float Height => -Bounce.Offset.Y + GroundHeight;
        public Vector2 Facing => Physics.Facing;
        public float GroundHeight { get; set; } = 0;

        public Character(IceGame level, PlayerInfo player, CharacterGraphics graphics, CharacterSound sound, Vector2 position)
        {
            Level = level;
            Player = player;
            Graphics = graphics;
            Sound = sound;

            Physics = new CharacterPhysics(position);
            Settings = new CharacterSettings();
            Bounce = new CharacterBounce();
            SnowballGathering = new CharacterSnowballGathering();

            State = new CharacterWalkState();
        }

        public void Update(CharacterInput input, float delta)
        {
            Bounce.Update(delta);
            Sound.Update(this, delta);
            State = State.Update(this, input, delta);
        }

        public void Draw(Graphics2D graphics, CharacterGraphics penguinGraphics)
        {
            if (Grounded)
            {
                penguinGraphics.DrawShadow(graphics, Position, GroundHeight);
            }

            State.Draw(graphics, this, penguinGraphics);
        }

        public void Bonk(Vector2 velocity)
        {
            State = new CharacterBonkState(velocity);
        }
        public void Drown()
        {
            State = new CharacterDrownState();
        }
    }
}
