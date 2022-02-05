using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace TinyGames.Engine.Maths.Algorithms.SquarePacking
{
    public class QuadTreeNode
    {
        public Rectangle Rectangle { get; private set; }
        public bool Available { get; set; } = true;
        public QuadTreeNode[] Children { get; private set; }
        public QuadTreeNode TopLeft => Children[0];
        public QuadTreeNode TopRight => Children[1];
        public QuadTreeNode BottomLeft => Children[2];
        public QuadTreeNode BottomRigth => Children[3];

        public bool IsLeaf => !HasChildren;
        public bool HasChildren => Children != null && Children.Length > 0;

        public QuadTreeNode(int x, int y, int width, int height) : this(new Rectangle(x, y, width, height)) { }

        public QuadTreeNode(Rectangle rectangle)
        {
            Rectangle = rectangle;
            Debug.Assert(Tools.IsPowerOfTwo(rectangle.Width));
            Debug.Assert(Tools.IsPowerOfTwo(rectangle.Height));
        }

        private void Subdivide()
        {
            if (HasChildren) throw new ArgumentException("Already subdivided");

            int x = Rectangle.X;
            int y = Rectangle.Y;
            int w = Rectangle.Width / 2;
            int h = Rectangle.Height / 2;

            Children = new QuadTreeNode[4];
            Children[0] = new QuadTreeNode(x, y, w, h);
            Children[1] = new QuadTreeNode(x + w, y, w, h);
            Children[2] = new QuadTreeNode(x, y + h, w, h);
            Children[3] = new QuadTreeNode(x + w, y + h, w, h);
        }

        public QuadTreeNode GetAvailableNode(int width, int height)
        {
            int halfWidth = Rectangle.Width / 2;
            int halfHeight = Rectangle.Height / 2;

            // We must use a child
            if(width <= halfWidth && height <= halfHeight)
            {
                return GetAvailableChildNode(width, height);
            }

            // We must use ourself, because we can't subdivide
            else
            {
                if (HasChildren) return null;
                if (!Available) return null;

                return this;
            }
        }

        private QuadTreeNode GetAvailableChildNode(int width, int height)
        {
            if (!HasChildren) Subdivide();

            foreach(var child in Children)
            {
                var node = child.GetAvailableNode(width, height);

                if (node != null) return node;
            }

            return null;
        }
    }

    public class QuadTreeSquarePackingAlgorithm : ISquarePackingAlgorithm
    {
        public Rectangle[] Pack(int width, int height, Point[] rectangleSizes)
        {
            QuadTreeNode root = new QuadTreeNode(0, 0, width, height);

            return rectangleSizes.Select(rect =>
            {
                var node = root.GetAvailableNode(rect.X, rect.Y);

                if (node == null) throw new ArgumentException("Cannot fit rectangle into blob");

                return node;
            }).Select(node => node.Rectangle).ToArray();
        }
    }
}
