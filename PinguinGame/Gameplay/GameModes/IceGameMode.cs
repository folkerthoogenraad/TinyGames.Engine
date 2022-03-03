using PinguinGame.GameStates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Gameplay.GameModes
{
    public abstract class IceGameMode<T> : GameMode
    {
        private T _result;

        public void SetResult(T result)
        {
            _result = result;
        }

        public T GetResult()
        {
            if (!Done) throw new InvalidOperationException("Cannot get result when not already done.");

            return _result;
        }
    }
}
