using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.Model.Shapes
{
    [Serializable]
    public class UMLClass : Shape
    {

        public UMLClass(double X, double Y, string Name) : base(X, Y, Name)
        {

        }

        // For XML Serialization
        public UMLClass() { }

    }
}
