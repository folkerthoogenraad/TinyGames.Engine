using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Util;

namespace PinguinGame.MiniGames.Ice.Behaviours
{
    public class CharacterCollisionBehaviour : ISceneBehaviour
    {
        public Scene Scene { get; set; }
        public IEnumerable<Character> Characters => Scene.FindGameObjectsOfType<Character>();
        private IEnumerable<Snowball> Snowballs => Scene.FindGameObjectsOfType<Snowball>();
        private IEnumerable<Geyser> Geysers => Scene.FindGameObjectsOfType<Geyser>();

        public void TryBonkCharacters()
        {
            // Bonks!
            foreach (var (a, b) in Characters.Combinations())
            {
                if (!a.CanCollide) continue;
                if (!b.CanCollide) continue;

                var p1 = a.Position;
                var p2 = b.Position;

                var dir = p2 - p1;
                var dist = dir.Length();

                if (dist > 8) continue;
                if (dist == 0)
                {
                    dir = new Vector2(1, 0);
                }
                else
                {
                    dir /= dist;
                }


                //Unstuck
                float penetration = (8 - dist) / 2;

                a.Position -= dir * penetration;
                b.Position += dir * penetration;

                var totalVelocity = Math.Abs(Vector2.Dot(a.Physics.Velocity, dir)) + Math.Abs(Vector2.Dot(b.Physics.Velocity, dir));
                var bonkVelocity = Math.Max(16, totalVelocity);

                var bonkA = BonkCharacters(a, b, bonkVelocity, -dir);
                var bonkB = BonkCharacters(b, a, bonkVelocity, dir);

                if (bonkA.Bonking) a.Bonk(bonkA.Velocity, bonkA.StunTime);
                if (bonkB.Bonking) b.Bonk(bonkB.Velocity, bonkB.StunTime);

                if (bonkA.Bonking || bonkB.Bonking) b.Sound.PlayBonk();
            }

            foreach (var character in Characters)
            {
                if (!character.CanCollide) continue;

                foreach (var snowball in Snowballs)
                {
                    if (character.Player == snowball.Info) continue;

                    var p1 = character.Position;
                    var p2 = snowball.Position;

                    var dir = p2 - p1;
                    var dist = dir.Length();

                    if (dist > 8) continue;

                    snowball.Collided = true;

                    character.Bonk(snowball.Velocity * 0.6f, 0.8f);
                    character.Sound.PlaySnowHit();
                }
            }

            foreach (var character in Characters)
            {
                if (!character.CanCollide) continue;

                foreach (var geyser in Geysers)
                {
                    if (!geyser.Erupting) continue;

                    var p1 = character.Position;
                    var p2 = geyser.Position;

                    var dir = p2 - p1;
                    var dist = dir.Length();

                    if (dist > 8) continue;
                    if (dist == 0) continue;

                    character.Bonk(-dir / dist * 64);
                    character.Bounce.Velocity = 128;
                    character.Sound.PlaySnowHit(); // TODO Geyser sounds and stuff
                }
            }
        }

        private (bool Bonking, Vector2 Velocity, float StunTime) BonkCharacters(Character self, Character other, float bonkVelocity, Vector2 direction)
        {
            // Advantage
            if (self.IsSliding && (other.IsGathering || other.IsWalking || self.IsBonking))
            {
                return (true, bonkVelocity * 0.3f * direction, 1f);
            }

            // Disadvantage
            else if ((self.IsGathering || self.IsWalking || self.IsBonking) && other.IsSliding)
            {
                return (true, bonkVelocity * (self.IsGathering ? 1.5f : 0.7f) * direction, 0.3f);
            }

            // Both bad :)
            else if (self.IsSliding && other.IsSliding)
            {
                return (true, bonkVelocity * 0.5f * direction, 1);
            }

            // No bonks
            else
            {
                // Divide equally
                return (false, bonkVelocity * 0.5f * direction, 0);
            }
        }

        public List<Character> TryDrownCharacters()
        {
            var result = new List<Character>();

            foreach (var p in Characters.Where(x => !x.IsDrowning))
            {
                if (!p.Grounded && p.Height < 1)
                {
                    p.Drown();
                    result.Add(p);
                }
            }

            return result;
        }

        // ========================================= //
        // Scene component stuff
        // ========================================= //
        public void Init(Scene scene)
        {
            Scene = scene;
        }

        public void BeforeUpdate(float delta)
        {

        }

        public void AfterUpdate(float delta)
        {

        }

        public void Destroy()
        {

        }
    }
}
