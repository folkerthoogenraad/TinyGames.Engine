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
        Center
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
            
        }

        private Vector2 GetAnchor(UIAnchor anchor)
        {
            switch (anchor)
            {
                case UIAnchor.TopLeft: return _localBounds.TopLeft;
                case UIAnchor.TopRight: return _localBounds.TopRight;
                case UIAnchor.BottomLeft: return _localBounds.BottomLeft;
                case UIAnchor.BottomRight: return _localBounds.BottomRight;
            };

            return _localBounds.Center;
        }
    }
}
