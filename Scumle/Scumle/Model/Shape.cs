using Scumle.Model.Shapes;
using Scumle.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Scumle.Model
{
 
    public class Shape : ModelBase
    {
        private double _width;
        private double _height;
        private Brush _shapeColor;
                
        public Shape(double X, double Y, String Name)
        {
            this.Width = 50;
            this.Height = 50;
            this.X = X;
            this.Y = Y;
            this.Name = Name;
            InitializeConnectionPoints();
         
        }

        private void InitializeConnectionPoints()
        {
            ConnectionPoints = new List<ConnectionPoint>()
            {
                new ConnectionPoint(this, HorizontalAlignment.Center, VerticalAlignment.Top),
                new ConnectionPoint(this, HorizontalAlignment.Left, VerticalAlignment.Center),
                new ConnectionPoint(this, HorizontalAlignment.Right, VerticalAlignment.Center),
                new ConnectionPoint(this, HorizontalAlignment.Center, VerticalAlignment.Bottom)
            };
        }

        // For XML Serialization
        public Shape() { }


        public double X { get; set; }
        public double Y { get; set; }
        public IList<ConnectionPoint> ConnectionPoints { get; private set; }
        public double Width
        {
            get { return _width; }
            set { if (value > 0) _width = value; }
        }
        public double Height
        {
            get { return _height; }
            set { if (value > 0) _height = value; }
        }
        public String Name { get; set; }

        public Brush ShapeColor
        {
            get { return _shapeColor; }
            set { _shapeColor = value; }
        }

        public void MoveDelta(double X, double Y)
        {
            this.X += X;
            this.Y += Y;
        }

        internal void Resize(double dX, double dY)
        {
            Width += dX;
            Height += dY;
        }
    }
}
