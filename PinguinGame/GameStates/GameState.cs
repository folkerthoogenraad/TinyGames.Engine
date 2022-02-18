using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Util;

namespace PinguinGame.GameStates
{
    public abstract class GameState : IGameState
    {
        public virtual void Init() { }
        public virtual void Update(float delta) { }
        public virtual void Draw(Graphics2D graphics) { }
        public virtual void Destroy() { }

        private SyncAwaitable _awaitable = new SyncAwaitable();
        public void Complete()
        {
            _awaitable.Complete();
        }
        public async Task WaitFor()
        {
            await _awaitable;
        }
    }
    public abstract class GameState<T> : IGameState
    {
        public virtual void Init() { }
        public virtual void Update(float delta) { }
        public virtual void Draw(Graphics2D graphics) { }
        public virtual void Destroy() { }

        private SyncAwaitable<T> _awaitable = new SyncAwaitable<T>();
        public void Complete(T result)
        {
            _awaitable.Complete(result);
        }
        public async Task<T> WaitFor()
        {
            return await _awaitable;
        }
    }
}
