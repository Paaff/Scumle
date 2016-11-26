using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.Model
{
    public class UMLItem : ModelBase
    { 

        public UMLItem(string s)
        {
            ListValue = s;
        }

        // For Serialization
        public UMLItem()  { }

        public string ListValue { get; set; }

    }
}

