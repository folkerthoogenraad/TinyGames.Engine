﻿using Microsoft.Xna.Framework;
using PinguinGame.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Extensions;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Scenes.Extensions;

namespace PinguinGame.MiniGames.Ice
{
    public class Grass : IceDetail
    {
        public Grass(Vector2 position) : base(position)
        {
        }

        public override void LoadSprites(IceGameGraphics graphics)
        {
            var random = new Random();

            Sprite = random.Choose(graphics.Grass0, graphics.Grass1, graphics.Grass2, graphics.Grass3);
        }
    }
}