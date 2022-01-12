using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Screens.Fades;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace PinguinGame.Screens
{
    public class ScreenManager
    {
        private Screen _nextScreen;
        private Screen _currentScreen;

        public Screen Screen {
            get => _currentScreen;
            set
            {
                _nextScreen = value;
                _fadeTimer = FadeOutDuration;
            }
        }
        private float _fadeTimer = 0;

        public float FadeOutDuration { get; set; } = 0.5f;
        public float FadeOutStartOffset { get; set; } = 0.2f;
        public float FadeInDuration { get; set; } = 0.3f;

        public bool IsFadingOut => _fadeTimer > 0 && _nextScreen != null;
        public bool IsFadingIn => _fadeTimer > 0 && _nextScreen == null;

        public float FadeFactor => GetFadeFactor();

        private GraphicsDevice _device;
        private ContentManager _content;
        private Graphics2D _graphics;

        private Fade _fadeIn;
        private Fade _fadeOut;

        public ScreenManager(GraphicsDevice device, ContentManager content)
        {
            _device = device;
            _content = content;
            _graphics = new Graphics2D(device);

            _fadeIn = new ColorFade(Color.Black);
            _fadeOut = new ColorFade(Color.Black);
        }

        public void Update(float delta)
        {
            if (IsFadingOut)
            {
                _fadeTimer -= delta;
                if(_fadeTimer < 0)
                {
                    _currentScreen?.Destroy();
                    _currentScreen = _nextScreen;
                    _nextScreen = null;
                    _currentScreen?.Init(_device, _content);
                    _fadeTimer = FadeInDuration;
                }

                _currentScreen?.UpdateAnimation(delta);
            }
            else if (IsFadingIn)
            {
                _fadeTimer -= delta;
                _currentScreen?.UpdateAnimation(delta);
            }
            else
            {
                _currentScreen?.Update(delta);
            }
        }

        public void Draw()
        {
            _currentScreen?.Draw();

            if(IsFadingIn || IsFadingOut)
            {
                _graphics.Begin(Matrix.Identity);

                var bounds = AABB.Create(-1, -1, 2, 2);

                if (IsFadingIn) _fadeIn.Draw(_graphics, FadeFactor, bounds);
                if (IsFadingOut) _fadeOut.Draw(_graphics, FadeFactor, bounds);

                _graphics.End();
            }
        }

        private float GetFadeFactor()
        {
            if (IsFadingIn)
            {
                return _fadeTimer / FadeInDuration;
            }
            else if (IsFadingOut)
            {
                if (_fadeTimer > FadeOutDuration - FadeOutStartOffset) return 0;
                return 1 - (_fadeTimer) / (FadeOutDuration - FadeOutStartOffset);
            }

            return 0;
        }
    }
}
