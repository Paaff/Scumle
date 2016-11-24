using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.Model.Shapes
{
    public class BasicShape : Shape
    {
        public BasicShape(EBasicShape type, double X, double Y) : base(X,Y)
        {
            Type = type;
        }

        public EBasicShape Type { get; set; }

        // For Serialization
        public BasicShape() { }
    }
}
