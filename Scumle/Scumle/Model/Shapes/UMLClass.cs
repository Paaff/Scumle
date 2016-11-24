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

        // An UML Class should carry a list of methods modelled and a list of fields modelled.
        public List<string> umlFields { get; set; }
        public List<string> umlMethods { get; set; }


        #region Constructor
        public UMLClass(double X, double Y) : base(X, Y)
        {
            umlFields = new List<string>()
            {
                "Field1",
                "Field2",
                "Field3"
            };

            umlMethods = new List<string>()
            {
                "Method1",
                "Method2",
                "Method3"
            };

        }
        // For XML Serialization
        public UMLClass() { }
        #endregion




    }
}
