using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using TinyGames.Engine.Graphics;

namespace PinguinGame.GameStates
{
    public abstract class GameMode
    {
        private IGameState _currentState;
        private IGameState State
        {
            get { return _currentState; }
            set
            {
                if (value == _currentState) return;
                _currentState?.Destroy();
                _currentState = value;
                _currentState?.Init();
            }
        }

        public bool Done { get; private set; } = false;
        private bool _started = false;

        public async void Run()
        {
            if (Done) throw new ArgumentException("Cannot run when already done");
            if (_started) throw new ArgumentException("Is already started.");

            _started = true;

            try
            {
                await RunSelf();
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            Done = true;
        }
        public abstract Task RunSelf();

        public virtual void Update(float delta)
        {
            State?.Update(delta);
        }
        public virtual void Draw(Graphics2D graphics)
        {
            State?.Draw(graphics);
        }
        public async Task<T> GotoState<T>(GameState<T> state)
        {
            State = state;

            return await state.WaitFor();
        }
        public virtual void Destroy()
        {
            State?.Destroy();
        }
    }
}
