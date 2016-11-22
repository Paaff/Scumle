using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Scumle.Model
{
    public class ConnectionPoint : ModelBase
    {

        public ConnectionPoint(Shape _shape, HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            Shape = _shape;
            Horizontal = horizontal;
            Vertical = vertical;
        }

        public HorizontalAlignment Horizontal { get; private set; }
        public VerticalAlignment Vertical { get; private set; }
        public Shape Shape { get; private set; }

        public double CenterX
        {
            get { return getCenterX(); }
        }

        public double CenterY
        {
            get { return getCenterY(); }
        }

        private double getCenterX()
        {
            if (Horizontal == HorizontalAlignment.Right)
            {
                return Shape.X + Shape.Width;
            }
            else if (Horizontal == HorizontalAlignment.Center)
            {
                return Shape.X + Shape.Width / 2;
            }
            else
            {
                return Shape.X;
            }
        }

        private double getCenterY()
        {
            if (Vertical == VerticalAlignment.Bottom)
            {
                return Shape.Y + Shape.Height;
            }
            else if (Vertical == VerticalAlignment.Center)
            {
                return Shape.Y + Shape.Height / 2;
            }
            else
            {
                return Shape.Y;
            }
        }

    }
}
