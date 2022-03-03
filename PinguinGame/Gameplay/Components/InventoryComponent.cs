using Microsoft.Xna.Framework;
using PinguinGame.Gameplay.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Scenes;

namespace PinguinGame.Gameplay.Components
{
    public class InventoryComponent : Component
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

        public SnowballGameObject CreateSnowball(CharacterGameObject character, Vector2 direction)
        {
            RemoveSnowball();

            if (direction.LengthSquared() > 0)
            {
                direction.Normalize();
            }

            // Throw snowball
            return new SnowballGameObject(character.Player)
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
