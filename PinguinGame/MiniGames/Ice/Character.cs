﻿using Microsoft.Xna.Framework;
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
    [RequireSceneComponent(typeof(SceneGraphics))]
    [RequireSceneComponent(typeof(Walkables))]
    [RequireSceneComponent(typeof(IceGameUIGraphics))]
    public class Character : GameObject, IDrawable2D
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

        public bool Invunerable => InvunerableTime > 0;
        public float InvunerableTime { get; set;  } = 0;

        public bool Grounded { get; set; } = true;
        public float Lifetime { get; set; } = 0;

        public Walkables Walkables { get; set; }
        public IceGameUIGraphics UIGraphics { get; set; }
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

        public bool CanCollide => !Invunerable && !IsDrowning;

        public Character(IceGame level, PlayerInfo player, Vector2 position)
        {
            Level = level;
            Player = player;
            Graphics = player.CharacterInfo.Graphics;
            Sound = player.CharacterInfo.Sound;

            Physics = new CharacterPhysics(position);
            Settings = new CharacterSettings();
            Bounce = new CharacterBounce();
            SnowballGathering = new CharacterSnowballGathering();

            State = new CharacterWalkState();
        }

        public override void Init()
        {
            base.Init();

            var sceneGraphics = Scene.GetComponent<SceneGraphics>();
            sceneGraphics.AddDrawable(this);

            Walkables = Scene.GetComponent<Walkables>();
            UIGraphics = Scene.GetComponent<IceGameUIGraphics>();
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
            var result = Walkables.GetGroundInfo(Position);

            if (IsDrowning)
            {
                GroundHeight = 0;
                return;
            }

            if (GroundHeight > result.Height)
            {
                Bounce.Height += GroundHeight - result.Height;
            }

            GroundHeight = result.Height;
            Grounded = result.Solid;
        }
        public void MoveWithGround(float delta)
        {
            var result = Walkables.GetGroundInfo(Position);

            Position += result.Velocity * delta;
        }

        public void Bonk(Vector2 velocity, float duration = 1)
        {
            State = new CharacterBonkState(Level.Effects, velocity, duration);
        }
        public void Drown()
        {
            State = new CharacterDrownState();
        }

        public override void Destroy()
        {
            Sound.StopAll();

            var sceneGraphics = Scene.GetComponent<SceneGraphics>();
            sceneGraphics.RemoveDrawable(this);
        }
    }
}
