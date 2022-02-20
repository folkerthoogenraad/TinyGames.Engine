using Microsoft.Xna.Framework;
using PinguinGame.MiniGames.Ice.CharacterStates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Ice
{
    public abstract class CharacterAction<T>
    {
        public CharacterGameObject Character { get; set; }

        public CharacterAction(CharacterGameObject data)
        {
            Character = data;
        }

        public abstract void Update(float delta, T status);
    }

    public class CharacterActionsSet
    {
        public CharacterAction<bool> Primary;
        public CharacterAction<bool> Secondary;
        public CharacterAction<Vector2> Move;

        public void Update(float delta, CharacterInput input)
        {
            Primary?.Update(delta, input.Action);
            Secondary?.Update(delta, input.ActionSecondary);
            Move?.Update(delta, input.MoveDirection);
        }

        public CharacterActionsSet Clone()
        {
            return new CharacterActionsSet()
            {
                Primary = Primary,
                Secondary = Secondary,
                Move = Move
            };
        }
    }

    public class CharacterActionComponent
    {
        public CharacterActionsSet CurrentActions { get; set; }

        private Stack<CharacterActionsSet> _actionStack;

        public CharacterActionComponent()
        {
            _actionStack = new Stack<CharacterActionsSet>();
        }

        public void PushActions()
        {
            _actionStack.Push(CurrentActions);
            CurrentActions = CurrentActions?.Clone();
        }

        public void Update(float delta, CharacterInput input)
        {
            CurrentActions?.Update(delta, input);
        }

        public void PopActions()
        {
            CurrentActions = _actionStack.Pop();
        }
    }
}
