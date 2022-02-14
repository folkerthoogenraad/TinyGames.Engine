using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Ice
{
    public class CharacterSnowballGathering
    {
        private float _snowballTimer = 0;
        private float _maxGatherTime = 1.0f;

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

        public Snowball CreateSnowball(Character character, Vector2 direction)
        {
            RemoveSnowball();

            if (direction.LengthSquared() > 0)
            {
                direction.Normalize();
            }

            // Throw snowball
            return new Snowball(character.Player)
            {
                Position = character.Position + direction * 8,
                Velocity = direction * 128,
                Lifetime = 1,
                Height = character.GroundHeight + 8,
                Info = character.Player,
            };
        }
    }
}
