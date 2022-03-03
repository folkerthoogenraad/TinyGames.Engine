using Microsoft.Xna.Framework;
using PinguinGame.Graphics;
using PinguinGame.Gameplay.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Scenes.Extensions;

namespace PinguinGame.Gameplay.Behaviours
{
    [RequireSceneBehaviour(typeof(GraphicsSceneBehaviour))]
    [RequireSceneBehaviour(typeof(IceGameUIGraphics))]
    public class CharacterIndicatorBehaviour : SceneBehaviour, IDrawable2D
    {
        public IceGameUIGraphics UIGraphics { get; private set; }
        public GraphicsSceneBehaviour Graphics { get; private set; }

        public override void Init(Scene scene)
        {
            base.Init(scene);

            Graphics = scene.GetBehaviour<GraphicsSceneBehaviour>();
            UIGraphics = scene.GetBehaviour<IceGameUIGraphics>();

            Graphics.AddManualDrawable(this);
        }

        public override void Destroy()
        {
            base.Destroy();

            Graphics.RemoveManualDrawable(this);
        }


        public void Draw(Graphics2D graphics)
        {
            DrawPlayerIndicators(graphics, Scene.GameObjects.OfType<CharacterGameObject>());
        }
        public void DrawPlayerIndicators(Graphics2D graphics, IEnumerable<CharacterGameObject> characters)
        {
            foreach (var character in characters)
            {
                if (character.Lifetime < 2)
                {
                    DrawIndicatorFor(graphics, character);
                }
            }
        }

        public void DrawIndicatorFor(Graphics2D graphics, CharacterGameObject character)
        {
            graphics.DrawSprite(UIGraphics.IndicatorOutline, character.Position - new Vector2(0, 32 + character.Bounce.Height), 0, GraphicsHelper.YToDepth(character.Position.Y));
            graphics.DrawSprite(UIGraphics.Indicator, character.Position - new Vector2(0, 32 + character.Bounce.Height), 0, GraphicsHelper.YToDepth(character.Position.Y), character.Player.Color);
        }
    }
}
