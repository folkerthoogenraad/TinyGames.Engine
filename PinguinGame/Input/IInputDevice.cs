using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Input
{
    // Unused currently
    public interface IInputDevice<T>
    {
        public T Poll();
        public void Flush();
    }
}
