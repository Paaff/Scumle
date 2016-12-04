using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Scumle.Model.Shapes
{
    public class BasicShape : Shape
    {
        public BasicShape(EBasicShape type, double X, double Y, double Width, double Height, Color col, string ID) : base(X,Y,Width,Height,col,ID)
        {
            Type = type;
        }

        public EBasicShape Type { get; set; }

        // For Serialization
        public BasicShape() { }
    }
}
