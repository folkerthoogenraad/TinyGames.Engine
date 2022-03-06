using Microsoft.Xna.Framework;
using PinguinGame.Gameplay.GameObjects;
using PinguinGame.Player;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Scenes;

namespace PinguinGame.Gameplay.Components
{
    public class InventoryComponent : Component
    {
        public bool HasSnowball { get; set; }

        public void RemoveItem()
        {
            HasSnowball = false;
        }

        public GameObject ThrowItem(PlayerInfo owner, Vector2 position, float height, Vector2 direction)
        {
            RemoveItem();

            if (direction.LengthSquared() > 0)
            {
                direction.Normalize();
            }

            // Throw snowball
            return new SnowballGameObject()
            {
                Position = position + direction * 8,
                Velocity = direction * 128,
                Lifetime = 1,
                Height = height + 8,
                Info = owner,
            };
        }
    }
}
