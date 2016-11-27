using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Scumle.Model.Shapes
{
    [Serializable]
    public class UMLClass : Shape
    {

        // An UML Class should carry a list of methods modelled and a list of fields modelled.
        public string Name { get; set; }
        public string UMLFields { get; set; }
        public string UMLMethods { get; set; }



        #region Constructor
        public UMLClass(double X, double Y, string Name,Color col) : base(X, Y, col)
        {
            UMLFields = "-First field : int\n-Second Field : String";
            UMLMethods = "+First method()\n+Second method()";
            this.Name = Name;


        }
        // For XML Serialization
        public UMLClass() { }
        #endregion

       



    }
}
