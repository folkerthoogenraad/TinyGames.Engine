using Microsoft.Xna.Framework;
using PinguinGame.Screens.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Maths;
using TinyGames.Engine.UI;
using TinyGames.Engine.UI.Animations;

namespace PinguinGame.Screens.UI
{
    public class UICharacterLivesModel
    {
        public int MaxLives { get; set; } = 3;
        public int Lives { get; set; } = 0;
        public Color Color { get; set; } = Color.White;
        public Sprite Icon { get; set; }
    }


    public class UICharacterLives : UIComponent
    {
        public bool RightAlign = true;

        public Sprite Heart { get; set; }
        public Sprite HeartOutline { get; set; }
        public UICharacterLivesModel Model { get; set; }

        public UICharacterLives(Sprite heart, Sprite heartOutline, UICharacterLivesModel model)
        {
            Heart = heart;
            HeartOutline = heartOutline;
            Model = model;
        }

        public override void DrawSelf(Graphics2D graphics, AABB bounds)
        {
            base.DrawSelf(graphics, bounds);

            float direction = RightAlign ? 1 : -1;

            graphics.DrawSprite(Model.Icon, bounds.Center);

            Vector2 offset = new Vector2(16 * direction, 0);
            Vector2 dist = new Vector2(13 * direction, 0);

            for(int i = 0; i < Model.MaxLives; i++)
            {
                graphics.DrawSprite(HeartOutline, offset);

                if(i < Model.Lives)
                {
                    graphics.DrawSprite(Heart, offset, 0, 0, Model.Color);
                }

                offset += dist;
            }
        }

        public void SetModel(UICharacterLivesModel newModel)
        {
            if(Model.Lives != newModel.Lives)
            {
                // Animate
            }

            Model = newModel;
        }
    }

    public class UIInGameModel
    {
        public UICharacterLivesModel[] Characters;
    }


    public class UIInGame : UIComponent
    {
        public InGameResources Resources { get; set; }

        public UICharacterLives[] Characters;

        public UIInGameModel Model { get; set; }

        public UIInGame(InGameResources resources, UIInGameModel model)
        {
            Model = model;
            Resources = resources;

            Characters = model.Characters.Select(
                character => new UICharacterLives(resources.Heart, resources.HeartOutline, character)
            ).ToArray();

            foreach (var c in Characters) AddComponent(c);
        }

        public override void UpdateLayout(AABB bounds)
        {
            base.UpdateLayout(bounds);

            float margin = 22;

            Vector2[] positions = new Vector2[]
            {
                bounds.TopLeft + new Vector2(margin, margin),
                bounds.TopRight + new Vector2(-margin, margin),
                bounds.BottomLeft + new Vector2(margin, -margin),
                bounds.BottomRight + new Vector2(-margin, -margin),
            };

            bool[] rightAlign = new bool[] 
            { 
                true,
                false,
                true,
                false
            };

            for(int i = 0; i < Characters.Length; i++)
            {
                Characters[i].UpdateLayout(AABB.Create(positions[i], Vector2.Zero));
                Characters[i].RightAlign = rightAlign[i];
            }
        }

        public void SetModel(UIInGameModel model)
        {
            Model = model;

            for(int i = 0; i < Model.Characters.Length; i++)
            {
                Characters[i].SetModel(Model.Characters[i]);
            }
        }

        public void FadeIn()
        {
            float margin = 22;

            Vector2[] offsets = new Vector2[]
            {
                new Vector2(0, -margin),
                new Vector2(0, -margin),
                new Vector2(0, margin),
                new Vector2(0, margin),
            };

            for (int i = 0; i < Characters.Length; i++)
            {
                var characterAnimation = new UIEaseAnimation();
                characterAnimation.PositionAnimation.Animate(offsets[i], Vector2.Zero);

                Characters[i].SetAnimation(characterAnimation);
            }
        }
    }
}
