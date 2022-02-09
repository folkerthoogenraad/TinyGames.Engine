using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;
using TinyGames.Engine.UI.Animations;

namespace TinyGames.Engine.UI
{
    public abstract class UIComponent
    {
        public AABB Bounds { get; set; }
        public IEnumerable<UIComponent> Children => _children;

        private List<UIComponent> _children = new List<UIComponent>();

        public UIAnimation Animation { get; private set; }

        public bool Visible { get; set; } = true;

        public virtual void Update(float delta)
        {
            if (Animation != null)
            {
                Animation?.Update(delta);

                if (Animation.Done)
                {
                    SetAnimation(null);
                }
            }

            foreach (var child in Children) child.Update(delta);
        }

        public virtual void UpdateLayout(AABB bounds)
        {
            Bounds = bounds;

            foreach (var child in Children) child.UpdateLayout(AABB.Create(Vector2.Zero, bounds.Size));
        }

        public virtual void Draw(Graphics2D graphics)
        {
            if (!Visible) return;

            if (Animation != null && !Animation.Visible) return;

            graphics.Push();
            graphics.Translate(Bounds.Center);

            var centerBounds = AABB.CreateCentered(Vector2.Zero, Bounds.Size);

            if (Animation != null)
            {
                if (Animation.Position.HasValue) graphics.Translate(Animation.Position.Value);
                if (Animation.Scale.HasValue) graphics.Scale(Animation.Scale.Value);

                // TODO rotation
                // TODO alpha

                graphics.SetAlpha(Animation.Alpha);
            }

            foreach (var child in Children) child.Draw(graphics);
            DrawSelf(graphics, centerBounds);

            graphics.Pop();
        }

        public virtual void DrawSelf(Graphics2D graphics, AABB bounds)
        {
        }

        public void AddComponent(UIComponent component)
        {
            _children.Add(component);
        }

        public void SetAnimation(UIAnimation animation)
        {
            Animation = animation;
        }
    }

}
