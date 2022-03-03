using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Util
{
    public class SyncAwaitable<T>
    {
        public bool IsCompleted { get; private set; } = false;
        public Exception Exception { get; private set; }
        public T Result { get; private set; }

        private SyncAwaiter<T> _awaiter;

        public SyncAwaitable()
        {
            _awaiter = new SyncAwaiter<T>(this);
        }

        public SyncAwaiter<T> GetAwaiter()
        {
            return _awaiter;
        }

        public void Complete(T v)
        {
            IsCompleted = true;
            Result = v;

            _awaiter.Complete();
        }
        public void SetException(Exception e)
        {
            IsCompleted = true;
            Exception = e;

            _awaiter.Complete();
        }
    }
}
