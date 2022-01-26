﻿using Microsoft.Xna.Framework;
using PinguinGame.MiniGames.Ice.PenguinStates;
using PinguinGame.Player;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Ice
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
        public bool IsDrowning => State is PenguinDrownState;

        public float DrownTimer { get; set; } = 0;

        public PlayerInfo Player { get; private set; }
        public PenguinPhysics Physics { get; set; }
        public PenguinSettings Settings { get; private set; }
        public PenguinBounce Bounce { get; private set; }
        public CharacterGraphics Graphics { get; private set; }

        public Vector2 Position => Physics.Position;
        public Vector2 DrawPosition => Physics.Position + Bounce.Offset;
        public Vector2 Facing => Physics.Facing;

        public Penguin(PlayerInfo player, CharacterGraphics graphics, Vector2 position)
        {
            Player = player;
            Physics = new PenguinPhysics(position); // TODO make this not immutable, its kinda a pain and completely useless atm
            Settings = new PenguinSettings();
            Bounce = new PenguinBounce();

            State = new PenguinWalkState();
            Graphics = graphics;
        }

        public void Update(PenguinInput input, float delta)
        {
            Bounce.Update(delta);
            State = State.Update(this, input, delta);
        }

        public void Draw(Graphics2D graphics, CharacterGraphics penguinGraphics, float height)
        {
            Physics.Position += new Vector2(0, -height);
            penguinGraphics.DrawShadow(graphics, Position);
            State.Draw(graphics, this, penguinGraphics);
            Physics.Position += new Vector2(0, height);
        }

        public void Bonk(Vector2 velocity)
        {
            State = new PenguinBonkState(velocity);
        }
        public void Drown()
        {
            State = new PenguinDrownState();
        }
    }
}