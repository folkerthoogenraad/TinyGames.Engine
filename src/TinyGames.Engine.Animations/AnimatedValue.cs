using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Animations
{
    public class AnimatedValue<T>
    {
        public Func<T, T, float, T> LerpFunction { get; set; }
        public EaseAnimation Animation { get; set; }

        public T Value
        {
            get => _lerpedValue;
            set
            {
                _previousValue = _lerpedValue;
                _value = value;

                Animation.Restart();
            }
        }

        private T _lerpedValue;
        private T _value;
        private T _previousValue;

        public AnimatedValue(T value, Func<T, T, float, T> lerpFunction, EaseAnimation animation)
        {
            _value = value;
            _previousValue = value;
            _lerpedValue = value;

            Animation = animation;
            LerpFunction = lerpFunction;
        }

        public void Update(float delta)
        {
            if (Animation.Update(delta))
            {
                RecalculateLerpedValue();
            }
        }

        private void RecalculateLerpedValue()
        {
            _lerpedValue = LerpFunction(_previousValue, _value, Animation.Value);
        }

        public void Animate(T from, T to)
        {
            _previousValue = from;
            _lerpedValue = from;
            _value = to;
            Animation.Restart();
        }
    }
}
