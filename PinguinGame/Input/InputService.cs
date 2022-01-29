using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PinguinGame.Input
{
    public class InputState
    {
        public InputDeviceType Type { get; private set; }

        public Vector2 MoveDirection { get; private set; } = new Vector2(0, 0);
        public bool Action { get; private set; } = false;
        public bool ActionPressed { get; private set; } = false;
        public bool ActionReleased { get; private set; } = false;

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

        public InputState(InputDeviceType device)
        {
            Type = device;
        }

        public static InputState UpdateState(InputState previous, InputFrame frame)
        {
            return new InputState(previous.Type)
            {
                Action = frame.Action,
                ActionPressed = frame.Action && !previous.Action,
                ActionReleased = !frame.Action && previous.Action,

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

    public class InputFrame
    {
        public Vector2 MoveDirection { get; set; } = new Vector2(0, 0);
        public bool Action { get; set; } = false;
        public bool Back { get; set; } = false;
        public bool Start { get; set; } = false;
    }

    public class InputService
    {
        private InputState[] _inputStates;

        public IEnumerable<InputState> InputStates => _inputStates;

        public InputService()
        {
            _inputStates = new InputState[] { 
                new InputState(InputDeviceType.Gamepad0),
                new InputState(InputDeviceType.Gamepad1),
                new InputState(InputDeviceType.Gamepad2),
                new InputState(InputDeviceType.Gamepad3),
                new InputState(InputDeviceType.Keyboard0),
                new InputState(InputDeviceType.Keyboard1),
            };
        }

        public void Poll()
        {
            for(int i = 0; i < _inputStates.Length; i++)
            {
                // Update the frame/state
                var state = _inputStates[i];
                var frame = GetInputFrameForDevice(state.Type);

                _inputStates[i] = InputState.UpdateState(state, frame);
            }
        }

        private InputFrame GetInputFrameForDevice(InputDeviceType device)
        {
            switch (device)
            {
                case InputDeviceType.Keyboard0: return GetInputFrameFromKeyboard(Keyboard.GetState(), 0);
                case InputDeviceType.Keyboard1: return GetInputFrameFromKeyboard(Keyboard.GetState(), 1);
                case InputDeviceType.Gamepad0: return GetInputFrameFromGamepadState(GamePad.GetState(0));
                case InputDeviceType.Gamepad1: return GetInputFrameFromGamepadState(GamePad.GetState(1));
                case InputDeviceType.Gamepad2: return GetInputFrameFromGamepadState(GamePad.GetState(2));
                case InputDeviceType.Gamepad3: return GetInputFrameFromGamepadState(GamePad.GetState(3));

                default: throw new NotImplementedException();
            }
        }

        private InputFrame GetInputFrameFromGamepadState(GamePadState state)
        {
            return new InputFrame
            {
                Action = state.Buttons.A == ButtonState.Pressed,
                Back = state.Buttons.B == ButtonState.Pressed,
                Start = state.Buttons.Start == ButtonState.Pressed,
                MoveDirection = new Vector2(state.ThumbSticks.Left.X, -state.ThumbSticks.Left.Y)
            };
        }
        private InputFrame GetInputFrameFromKeyboard(KeyboardState state, int index)
        {
            Vector2 dir = new Vector2();

            if(index == 0)
            {
                if (state.IsKeyDown(Keys.Left)) dir.X -= 1;
                if (state.IsKeyDown(Keys.Right)) dir.X += 1;
                if (state.IsKeyDown(Keys.Up)) dir.Y -= 1;
                if (state.IsKeyDown(Keys.Down)) dir.Y += 1;

                if (dir.LengthSquared() > 1) dir.Normalize();

                return new InputFrame
                {
                    Action = state.IsKeyDown(Keys.Space),
                    Start = state.IsKeyDown(Keys.Enter),
                    Back = state.IsKeyDown(Keys.Back),
                    MoveDirection = dir
                };
            }
            else
            {
                if (state.IsKeyDown(Keys.NumPad4)) dir.X -= 1;
                if (state.IsKeyDown(Keys.NumPad6)) dir.X += 1;
                if (state.IsKeyDown(Keys.NumPad8)) dir.Y -= 1;
                if (state.IsKeyDown(Keys.NumPad5)) dir.Y += 1;

                if (dir.LengthSquared() > 1) dir.Normalize();

                return new InputFrame
                {
                    Start = state.IsKeyDown(Keys.C),
                    Action = state.IsKeyDown(Keys.X),
                    Back = state.IsKeyDown(Keys.Z),
                    MoveDirection = dir
                };
            }
        }

        public InputState GetInputStateForDevice(InputDeviceType device)
        {
            return _inputStates.Where(x => x.Type == device).FirstOrDefault();
        }
    }
}
