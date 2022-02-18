using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Util
{
    public class SyncAwaitable<T>
    {
        private SyncAwaiter<T> _awaiter;
        public SyncAwaitable()
        {
            _awaiter = new SyncAwaiter<T>();
        }

        public SyncAwaiter<T> GetAwaiter()
        {
            return _awaiter;
        }

        public void Complete(T v)
        {
            _awaiter.Complete(v);
        }
    }

    public class SyncAwaitable
    {
        private SyncAwaiter _awaiter;
        public SyncAwaitable()
        {
            _awaiter = new SyncAwaiter();
        }

        public SyncAwaiter GetAwaiter()
        {
            return _awaiter;
        }

        public void Complete()
        {
            _awaiter.Complete();
        }
    }
}
