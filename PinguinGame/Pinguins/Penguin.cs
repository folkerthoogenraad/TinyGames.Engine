using Microsoft.Xna.Framework;
using PinguinGame.Player;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.Pinguins
{
    public class Penguin
    {
        private PenguinState _state { get; set; }
        private PenguinState State
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

        public bool IsBonking => State is PenguinBonkState;
        public bool IsSliding => State is PenguinSlideState;
        public bool IsWalking => State is PenguinWalkState;

        public PlayerInfo Player { get; private set; }
        public PenguinPhysics Physics { get; set; }
        public PenguinSettings Settings { get; private set; }
        public PenguinBounce Bounce { get; private set; }

        // Penguin bounce stuff

        public Vector2 Position => Physics.Position;
        public Vector2 DrawPosition => Physics.Position + Bounce.Offset;
        public Vector2 Facing => Physics.Facing;

        public Penguin(PlayerInfo player)
        {
            Player = player;
            Physics = new PenguinPhysics(Vector2.Zero); // TODO make this not immutable, its kinda a pain and completely useless atm
            Settings = new PenguinSettings();
            Bounce = new PenguinBounce();

            State = new PenguinWalkState();
        }

        public void Update(PenguinInput input, float delta)
        {
            Bounce.Update(delta);
            State = State.Update(this, input, delta);

            Debug.WriteLine("Poisition : " + Position + ", Velocity : " + Physics.Velocity);
        }

        public void Draw(Graphics2D graphics, PenguinGraphics penguinGraphics)
        {
            penguinGraphics.DrawShadow(graphics, Position);
            State.Draw(graphics, this, penguinGraphics);
        }

        public void Bonk(Vector2 velocity)
        {
            State = new PenguinBonkState(velocity);
        }
    }
}
