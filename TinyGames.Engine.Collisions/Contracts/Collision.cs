using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Collisions.Contracts
{
    public class Collision
    {
        public int BodyA;
        public int BodyB;

        public Collision(int a, int b)
        {
            BodyA = a;
            BodyB = b;
        }
    }
}
