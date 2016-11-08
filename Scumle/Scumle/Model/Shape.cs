using Scumle.Model.Shapes;
using Scumle.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Xml.Serialization;

namespace Scumle.Model
{
   [XmlInclude(typeof(UMLClass))]
    [XmlInclude(typeof(Eclipse))]
    public class Shape
    {
        private double _width;
        private double _height;
                
        public Shape(double X, double Y, String Name)
        {
            this.Width = 25;
            this.Height = 25;
            this.X = X-Width/2;
            this.Y = Y-Height/2;
            this.Name = Name;
         
        }

        // For XML Serialization
        public Shape() { }


        public double X { get; set; }
        public double Y { get; set; }

        public double Width
        {
            get { return _width; }
            set
            {
                if (value < 0) return;
                _width = value;
            }
        }
        public double Height
        {
            get { return _height; }
            set
            {
                if (value < 0) return;
                _height = value;
            }
        }
        public String Name { get; set; }

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
