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
        public string Name { get; set; }
        public string UMLFields { get; set; }
        public string UMLMethods { get; set; }



        #region Constructor
        public UMLClass(double X, double Y, double Width, double Height, string Name,Color col, string ID, string fields, string methods) : base(X, Y, Width, Height, col, ID)
        {
            UMLFields = fields;
            UMLMethods = methods;          
            this.Name = Name;


        }
        // For XML Serialization
        public UMLClass() { }
        #endregion

       



    }
}
