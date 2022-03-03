using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Graphics;
using PinguinGame.Gameplay.Generic;
using PinguinGame.Gameplay.Behaviours;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Scenes.Extensions;
using TinyGames.Engine.Maths;

namespace PinguinGame.Gameplay.GameObjects
{
    [RequireSceneBehaviour(typeof(WalkablesSceneBehaviour))]
    public class ShoppingCartGameObject : GameObject, IDrawable2D
    {
        public ShoppingCartGraphics Graphics { get; set; }
        public Vector2 Facing { get; set; } = new Vector2(1, 0);
        public Vector2 Position { get; set; } = new Vector2(0, 0);
        public Vector2 Velocity { get; set; } = new Vector2(0, 0);
        public WalkablesSceneBehaviour Walkables { get; set; }
        public float Height = 0;

        public bool HasController => Controller != null;
        public CharacterGameObject Controller { get; set; }

        public ShoppingCartGameObject(ContentManager content, Vector2 position)
        {
            Graphics = new ShoppingCartGraphics(content.Load<Texture2D>("Sprites/Vehicles/ShoppingCart"));

            Position = position;
        }

        public override void Init()
        {
            base.Init();

            Walkables = Scene.GetBehaviour<WalkablesSceneBehaviour>();
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            // Very stupid crude friction

            float speed = Velocity.Length();

            if(speed > 0)
            {
                speed -= speed * speed * 0.0001f;

                if(speed < 0) speed = 0;

                Velocity = Velocity.Normalized() * speed;
            }

            Position += Velocity * delta;

            PlaceOnGround();
        }

        public void PlaceOnGround()
        {
            var result = Walkables.GetGroundInfo(Position);

            Height = result.Height;
        }

        public void Draw(Graphics2D graphics)
        {
            var g = Graphics.GetGraphicsForFacing(ShoppingCartGraphics.GetFacingFromVector(Facing));


            var offset = new Vector2(0, 8);
            var position = Position + offset;

            graphics.DrawSprite(g.Cart, position - new Vector2(0, Height), 0, GraphicsHelper.YToDepth(position.Y - 8), Color.White);
            graphics.DrawSprite(g.CartOverlay, position - new Vector2(0, Height), 0, GraphicsHelper.YToDepth(position.Y), Color.White);
        }
    }
}
