using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Animations
{

    public class EaseAnimation
    {
        public float PreDelay { get; set; } = 0;
        public float CurrentTime { get; set; } = 0;
        public float Duration { get; set; } = 1;

        public float Value => _easeFunction(GetLinearValue());

        private Func<float, float> _easeFunction;

        public EaseAnimation(Func<float, float> easeFunction, float duration, float predelay = 0)
        {
            _easeFunction = easeFunction;
            Duration = duration;
            PreDelay = predelay;
        }

        public bool Done => CurrentTime >= Duration + PreDelay;

        public void Restart()
        {
            CurrentTime = 0;
        }

        public bool Update(float delta)
        {
            if (Done) return false;

            CurrentTime += delta;

            return true;
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
