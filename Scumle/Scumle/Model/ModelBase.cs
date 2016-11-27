using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Scumle.Model.Shapes;

namespace Scumle.Model
{   
    [XmlInclude(typeof(BasicShape))]
    [XmlInclude(typeof(UMLClass))]
    [XmlInclude(typeof(Line))]
    [XmlInclude(typeof(ConnectionPoint))] // Maybe not used.
    public class ModelBase
    {

    }
}
