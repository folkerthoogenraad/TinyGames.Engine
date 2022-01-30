using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Input
{
    /// <summary>
    /// The input frame. The raw data from the device
    /// </summary>
    public class InputFrame
    {
        public Vector2 MoveDirection { get; set; } = new Vector2(0, 0);
        public bool Action { get; set; } = false;
        public bool ActionSecondary { get; set; } = false;
        public bool Back { get; set; } = false;
        public bool Start { get; set; } = false;
    }
}
