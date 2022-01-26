using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Collisions.Contracts;
using TinyGames.Engine.Collisions.Detectors;
using TinyGames.Engine.Collisions.Solvers;

namespace TinyGames.Engine.Collisions
{
    public class PhysicsWorld
    {
        public IEnumerable<PhysicsBody> Bodies => _bodies;

        private IDetector _detector;
        private ISolver _solver;
        private List<PhysicsBody> _bodies;

        public PhysicsWorld()
        {
            _bodies = new List<PhysicsBody>();
            _solver = new DefaultSolver();
            _detector = new SweepAndPruneDetector();
        }

        public void AddBody(PhysicsBody body)
        {
            _bodies.Add(body);
        }

        public void RemoveBody(PhysicsBody body)
        {
            _bodies.Remove(body);
        }

        public BodyCollisionSet Update(float delta)
        {
            // Update positions
            foreach(var body in _bodies)
            {
                if (body.Static) continue;

                body.Position += body.Velocity * delta;
            }

            // Solve the collisions
            var collisions = _detector.Detect(_bodies);
            _solver.Solve(collisions);

            // Get the unstuck motion
            foreach(var collision in collisions.CollisionIndices)
            {
                var from = collision.BodyA;
                var to = collision.BodyB;

                from.Body.Position = from.Position + from.UnstuckMotion;
                to.Body.Position = to.Position + to.UnstuckMotion;
            }

            return collisions;
        }
    }
}
