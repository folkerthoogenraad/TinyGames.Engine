using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace TinyGames.Engine.Util
{
    public class SyncAwaiter<T> : INotifyCompletion
    {
        // TODO can this only be one? I don't think so right?
        public bool IsCompleted => _awaitable.IsCompleted;
        private Action _onCompletion;
        private SyncAwaitable<T> _awaitable;

        public SyncAwaiter(SyncAwaitable<T> awaitable)
        {
            _awaitable = awaitable;
        }

        public void OnCompleted(Action continuation)
        {
            if (_onCompletion != null) throw new NotImplementedException("Cannot add another conitnuation to this awaitable.");

            _onCompletion = continuation;
        }

        public void Complete()
        {
            _onCompletion?.Invoke();
        }

        public T GetResult()
        {
            if (!_awaitable.IsCompleted) throw new InvalidOperationException("Cannot get result from uncompleted awaitable.");
            if (_awaitable.Exception != null) throw _awaitable.Exception;
            
            return _awaitable.Result;
        }
    }
}
