using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Maths;

namespace TinyGames.Engine.UI
{
    public enum UIAnchor
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        
        Center,
        
        TopCenter,
        BottomCenter,
        LeftCenter,
        RightCenter,
    }

    // TOOD this class is unused
    public class UILayout
    {
        private UIComponent _component;
        private AABB _localBounds;

        public UILayout(UIComponent component, AABB bounds)
        {
            _component = component;
            _localBounds = AABB.CreateCentered(Vector2.Zero, bounds.Size);
        }

        public void Anchor(UIComponent child, UIAnchor anchor, Vector2 offset)
        {
            Anchor(child, anchor, AABB.Create(offset, Vector2.Zero));
        }

        public void Anchor(UIComponent child, UIAnchor anchor, AABB bounds)
        {
            child.UpdateLayout(bounds.Translated(GetAnchor(anchor)));
        }

        public void Overlay(UIComponent child, float inset = 0)
        {
            child.UpdateLayout(_localBounds.Shrink(inset));
        }

        private Vector2 GetAnchor(UIAnchor anchor)
        {
            switch (anchor)
            {
                case UIAnchor.TopLeft: return _localBounds.TopLeft;
                case UIAnchor.TopRight: return _localBounds.TopRight;
                case UIAnchor.BottomLeft: return _localBounds.BottomLeft;
                case UIAnchor.BottomRight: return _localBounds.BottomRight;

                case UIAnchor.Center: return _localBounds.Center;

                case UIAnchor.TopCenter: return _localBounds.TopCenter;
                case UIAnchor.BottomCenter: return _localBounds.BottomCenter;
                case UIAnchor.LeftCenter: return _localBounds.LeftCenter;
                case UIAnchor.RightCenter: return _localBounds.RightCenter;
            };

            return _localBounds.Center;
        }
    }
}
