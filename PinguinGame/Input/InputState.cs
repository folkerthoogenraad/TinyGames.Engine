using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Input
{
    /// <summary>
    /// The input state, with pressed and released events.
    /// </summary>
    public class InputState
    {
        public InputDeviceType Type { get; private set; }
        public Vector2 MoveDirection { get; private set; } = new Vector2(0, 0);
        public bool Action { get; private set; } = false;
        public bool ActionPressed { get; private set; } = false;
        public bool ActionReleased { get; private set; } = false;

        public bool ActionSecondary { get; private set; } = false;
        public bool ActionSecondaryPressed { get; private set; } = false;
        public bool ActionSecondaryReleased { get; private set; } = false;

        public bool Back { get; private set; } = false;
        public bool BackPressed { get; private set; } = false;
        public bool BackReleased { get; private set; } = false;

        public bool Start { get; private set; } = false;
        public bool StartPressed { get; private set; } = false;
        public bool StartReleased { get; private set; } = false;

        public bool MenuUpPressed { get; private set; }
        public bool MenuDownPressed { get; private set; }
        public bool MenuLeftPressed { get; private set; }
        public bool MenuRightPressed { get; private set; }

        public InputState(InputDeviceType type)
        {
            Type = type;
        }

        public static InputState CreateFromPreviousAndFrame(InputState previous, InputFrame frame)
        {
            return new InputState(previous.Type)
            {
                Action = frame.Action,
                ActionPressed = frame.Action && !previous.Action,
                ActionReleased = !frame.Action && previous.Action,

                ActionSecondary = frame.ActionSecondary,
                ActionSecondaryPressed = frame.ActionSecondary && !previous.ActionSecondary,
                ActionSecondaryReleased = !frame.ActionSecondary && previous.ActionSecondary,

                Back = frame.Back,
                BackPressed = frame.Back && !previous.Back,
                BackReleased = !frame.Back && previous.Back,

                Start = frame.Start,
                StartPressed = frame.Start && !previous.Start,
                StartReleased = !frame.Start && previous.Start,

                MoveDirection = frame.MoveDirection,

                MenuUpPressed = previous.MoveDirection.Y > -0.5f && frame.MoveDirection.Y <= -0.5f,
                MenuDownPressed = previous.MoveDirection.Y < 0.5f && frame.MoveDirection.Y >= 0.5f,
                MenuRightPressed = previous.MoveDirection.X < 0.5f && frame.MoveDirection.X >= 0.5f,
                MenuLeftPressed = previous.MoveDirection.X > -0.5f && frame.MoveDirection.X <= -0.5f,
            };
        }
    }
}
