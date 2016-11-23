using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.Model.Shapes
{
    class Rectangle : Shape
    {
        public Rectangle(double X, double Y, string Name) : base(X, Y, Name)
        {
        }

        // For XML Serialization
        public Rectangle() { }
    }
}
