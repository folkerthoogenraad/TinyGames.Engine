using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Collisions.Contracts;
using TinyGames.Engine.Collisions.Detectors;
using TinyGames.Engine.Collisions.Solvers;

namespace TinyGames.Engine.Collisions
{
    public class World
    {
        public IEnumerable<Body> Bodies => _bodies;

        private IDetector _detector;
        private ISolver _solver;
        private List<Body> _bodies;

        public World()
        {
            _bodies = new List<Body>();
            _solver = new DefaultSolver();
            _detector = new SweepAndPruneDetector();
        }

        public void AddBody(Body body)
        {
            _bodies.Add(body);
        }

        public void RemoveBody(Body body)
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
            var collisions = _detector.Solve(_bodies);
            _solver.Solve(collisions);

            // Get the unstuck motion
            foreach(var collision in collisions.CollisionIndices)
            {
                var from = collisions.Bounds[collision.BodyA];
                var to = collisions.Bounds[collision.BodyB];

                from.Body.Position = from.Position + from.UnstuckMotion;
                to.Body.Position = to.Position + to.UnstuckMotion;
            }

            return collisions;
        }
    }
}
