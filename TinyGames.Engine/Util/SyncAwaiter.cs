using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace TinyGames.Engine.Util
{
    public class SyncAwaiter<T> : SyncAwaiter
    {
        private T Result;

        public void Complete(T result)
        {
            Result = result;
            Complete();
        }

        public T GetResult()
        {
            return Result;
        }
    }
    public class SyncAwaiter : INotifyCompletion
    {
        public bool IsCompleted { get; set; } = false;
        private List<Action> _onCompletionList;

        public SyncAwaiter()
        {
            _onCompletionList = new List<Action>();
        }

        public void OnCompleted(Action continuation)
        {
            _onCompletionList.Add(continuation);
        }

        public void Complete()
        {
            IsCompleted = true;
            foreach (var action in _onCompletionList) action();
        }
        public void GetResult()
        {
            // nothing :)
        }
    }
}
