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

        public CollisionSet CollisionSet;

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

        public void Update(float delta)
        {
            // Update positions
            foreach(var body in _bodies)
            {
                body.Position += body.Velocity * delta;
            }

            CollisionSet = _detector.Solve(_bodies);
            _solver.Solve(CollisionSet);

            foreach(var collision in CollisionSet.Collisions)
            {
                var from = CollisionSet.Bounds[collision.BodyA];
                var to = CollisionSet.Bounds[collision.BodyB];

                from.Body.Position = from.Position + from.UnstuckMotion;
                to.Body.Position = to.Position + to.UnstuckMotion;
            }
        }
    }
}
