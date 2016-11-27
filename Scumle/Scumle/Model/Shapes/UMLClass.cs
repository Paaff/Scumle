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
        public List<UMLItem> umlFields { get; set; }
        public List<UMLItem> umlMethods { get; set; }
        public string Name { get; set; }
      


        #region Constructor
        public UMLClass(double X, double Y, string Name,Color col) : base(X, Y, col)
        {
            umlFields = new List<UMLItem>()
            {
                new UMLItem("Field1"),
                new UMLItem("Field2"),
                new UMLItem("Field3")
            };

            umlMethods = new List<UMLItem>()
            {
                new UMLItem("Method1"),
                new UMLItem("Method2"),
                new UMLItem("Method3")
            };
            this.Name = Name;


        }
        // For XML Serialization
        public UMLClass() { }
        #endregion

       



    }
}
