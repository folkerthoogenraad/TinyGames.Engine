using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StudentBikeGame.Games;
using StudentBikeGame.Games.Bike;
using StudentBikeGame.Games.Obstacles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Extensions;
using TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators;
using StudentBikeGame.Games.UI;
using TinyGames.Engine.Graphics.Fonts;

namespace StudentBikeGame.Screens
{
    public class InGameScreen : Screen
    {
        public BikeWorld World;
        public BikeController BikeController;

        public CoinGraphics CoinGraphics;
        public List<Coin> Coins { get; set; }
        public ParticleSystem Particles { get; set; }

        public int Score = 0;

        public InGameUI UI;
        public List<FlashText> FlashTexts { get; set; }

        private Font SmallFont;

        public override void Init(GraphicsDevice device, ContentManager content)
        {
            base.Init(device, content);

            var sheet = content.Load<Texture2D>("Sprites/Sheet");
            var background = content.Load<Texture2D>("Sprites/Background");
            var backgroundCollision = content.Load<Texture2D>("Sprites/Background_Collision");


            var bikeGraphicsSettings = new BikeGraphicsSettings() 
            {
                BikeSprite = new Sprite(sheet, new Rectangle(0, 0, 32, 16)).CenterOrigin(),
                BikeTurnSprite = new Sprite(sheet, new Rectangle(32, 0, 32, 16)).CenterOrigin(),

                BikerAnimation = new Animation(
                    new Sprite(sheet, new Rectangle(0, 16, 32, 16)).CenterOrigin(),
                    new Sprite(sheet, new Rectangle(0, 32, 32, 16)).CenterOrigin()).SetFrameRate(1),

                BikerTurnAnimation = new Animation(
                    new Sprite(sheet, new Rectangle(32, 16, 32, 16)).CenterOrigin()).SetFrameRate(1),
            };

            var coinGraphics = new CoinGraphics()
            {
                Sprite = new Sprite(sheet, new Rectangle(80, 16, 16, 16)).CenterOrigin(),

                PickupParticle = new Animation(
                    new Sprite(sheet, new Rectangle(64 + 00, 32, 16, 16)).CenterOrigin(),
                    new Sprite(sheet, new Rectangle(64 + 16, 32, 16, 16)).CenterOrigin(),
                    new Sprite(sheet, new Rectangle(64 + 32, 32, 16, 16)).CenterOrigin(),
                    new Sprite(sheet, new Rectangle(64 + 48, 32, 16, 16)).CenterOrigin()
                    ).SetFrameRate(8),
            };

            World = new BikeWorld(background, backgroundCollision, coinGraphics);
            BikeController = new BikeController(new Bike(new BikeHandling()), new BikeGraphics(bikeGraphicsSettings));

            Coins = new List<Coin>();
            Particles = new ParticleSystem();
            CoinGraphics = coinGraphics;

            FlashTexts = new List<FlashText>();

            // Generate coins
            var random = new Random();

            for(int i = 0; i < 100; i++)
            {
                Coins.Add(new Coin(random.NextPointInBox(0, 1024, 0, 1024)));
            }

            SmallFont = content.LoadFont("Fonts/Font5x6");
            var font = content.LoadFont("Fonts/Font8x10");
            var outlineFont = FontOutline.Create(device, font);

            UI = new InGameUI(font, outlineFont);
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            BikeController.Update(World, delta);
            Particles.Update(delta);

            Coins = Coins.Where(x => {
                if (x.Bounds.Overlaps(BikeController.Bounds))
                {
                    FlashTexts.Add(new FlashText(SmallFont, "+10", x.Position));
                    Score += 10;

                    for(int i = 0; i < 2; i++)
                    {
                        var random = new Random();

                        Particles.Add(new Particle() {
                            Position = x.Position + random.NextPointInCircle() * 8,
                            Color = Color.White,
                            Animation = CoinGraphics.PickupParticle
                        });
                    }

                    return false;
                }
                return true;
            }).ToList();

            // TODO move this?
            var targetPosition = BikeController.Bike.Position + BikeController.Bike.Heading * BikeController.Bike.ForwardVelocity;
            Camera.Position = Vector2.Lerp(Camera.Position, targetPosition, delta * 2);

            // Update the flashtexts
            FlashTexts = FlashTexts.Where(x => x.Update(delta)).ToList();

            UI.Update(Score, delta);
        }

        public override void Draw()
        {
            base.Draw();

            Graphics.Clear(Color.White);
            Graphics.Begin(Camera.GetMatrix());

            World.Draw(Graphics);

            foreach (var coin in Coins)
            {
                Graphics.DrawSprite(CoinGraphics.Sprite, coin.Position, 0, 0);
            }

            Particles.Draw(Graphics);

            BikeController.Draw(Graphics);

            foreach (var text in FlashTexts) text.Draw(Graphics);
            UI.Draw(Graphics, Camera.Bounds);

            Graphics.End();
        }
    }
}
