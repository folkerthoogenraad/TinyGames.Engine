using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Ice
{
    public class CharacterSnowball
    {
        private float _snowballTimer = 0;
        private float _maxGatherTime = 0.7f;

        public bool HasSnowball { get; private set; }
        public bool Gathering { get; private set; }
        public float GatherProgress => _snowballTimer / _maxGatherTime;

        public void RemoveSnowball()
        {
            HasSnowball = false;
            Gathering = false;
            _snowballTimer = 0;
        }

        public void Update(float delta, bool gathering)
        {
            if (HasSnowball) return;

            Gathering = gathering;

            if (!gathering)
            {
                _snowballTimer = 0;
                return;
            }

            _snowballTimer += delta;

            if(_snowballTimer > _maxGatherTime)
            {
                HasSnowball = true;
            }
        }
    }
}
