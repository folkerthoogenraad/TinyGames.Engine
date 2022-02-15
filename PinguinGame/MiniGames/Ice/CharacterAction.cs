using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Ice
{
    public class CharacterActionInfo
    {
        public bool CanPerform { get; set; }
        public string Name { get; set; }
    }
    public abstract class CharacterAction<T>
    {
        public abstract void Update(float delta, T status);
    }

    public abstract class CharacterButtonAction : CharacterAction<bool>
    {

    }
    public abstract class CharacterMoveAction : CharacterAction<Vector2>
    {

    }

    public class CharacterActionsSet
    {
        public CharacterButtonAction Primary;
        public CharacterButtonAction Secondary;
        public CharacterMoveAction Move;

        public void Update(float delta, CharacterInput input)
        {
            Primary.Update(delta, input.Action);
            Secondary.Update(delta, input.ActionSecondary);
            Move.Update(delta, input.MoveDirection);
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
        public CharacterActionsSet CurrentActions;

        private Stack<CharacterActionsSet> _actionStack;

        public void PushActions()
        {
            _actionStack.Push(CurrentActions);
            CurrentActions = CurrentActions.Clone();
        }

        public void PopActions()
        {
            CurrentActions = _actionStack.Pop();
        }
    }
}
