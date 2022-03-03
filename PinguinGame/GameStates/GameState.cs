using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Util;

namespace PinguinGame.GameStates
{
    public abstract class GameState<T> : IGameState
    {
        public virtual void Init() { }
        public virtual void Update(float delta) { }
        public virtual void Draw(Graphics2D graphics) { }
        public virtual void Destroy()
        {
            if (!_awaitable.IsCompleted)
            {
                _awaitable.SetException(new OperationCanceledException());
            }
        }

        private SyncAwaitable<T> _awaitable = new SyncAwaitable<T>();
        public void Complete(T result)
        {
            _awaitable.Complete(result);
        }
        public void SetException(Exception e)
        {
            _awaitable.SetException(e);
        }
        public async Task<T> WaitFor()
        {
            return await _awaitable;
        }
    }
}
