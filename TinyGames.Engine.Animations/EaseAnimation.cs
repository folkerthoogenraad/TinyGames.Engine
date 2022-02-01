using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Animations
{
    public class EaseAnimation
    {
        public float PreDelay { get; set; } = 0;
        public float CurrentTime { get; set; } = 0;
        public float Duration { get; set; }

        public float Value => _easeFunction(GetLinearValue());

        private Func<float, float> _easeFunction;

        public EaseAnimation(Func<float, float> easeFunction, float duration)
        {
            _easeFunction = easeFunction;
            Duration = duration;
        }

        public void Update(float delta)
        {
            CurrentTime += delta;
        }
        private float GetLinearValue()
        {
            return GetLinearValue(CurrentTime);
        }

        private float GetLinearValue(float time)
        {
            if (time < PreDelay) return 0;
            if (time > PreDelay + Duration) return 1;

            float v = time - PreDelay;

            return v / Duration;
        }
    }
}
