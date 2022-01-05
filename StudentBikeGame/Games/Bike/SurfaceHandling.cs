using System;
using System.Collections.Generic;
using System.Text;

namespace StudentBikeGame.Games.Bike
{
    public class SurfaceHandling
    {
        public float GripMultiplier { get; set; }
        public float ResistanceMultiplier { get; set; }

        public float SteepnessMuliplier { get; set; }

        public SurfaceHandling()
        {
            GripMultiplier = 1;
            ResistanceMultiplier = 1;
            SteepnessMuliplier = 20;
        }
    }

}
