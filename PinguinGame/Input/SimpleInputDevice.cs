using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Input
{
    public abstract class SimpleInputDevice<T, S> : IInputDevice<T> where T : class
    {
        private T _polledResult { get; set; }
        private S Previous { get; set; }

        public T Poll()
        {
            if (_polledResult != null) return _polledResult;

            S current = PollSimple();

            _polledResult = CreateFrom(current, Previous);

            Previous = current;

            return _polledResult;
        }

        public void Flush()
        {
            _polledResult = null;
        }

        public abstract S PollSimple();
        public abstract T CreateFrom(S current, S previous);
    }
}
