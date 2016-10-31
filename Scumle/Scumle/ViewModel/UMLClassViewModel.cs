using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scumle.Model;

namespace Scumle.ViewModel
{
    public class UMLClassViewModel : ShapeViewModel
    {
        public UMLClassViewModel(Shape shape) : base(shape)
        {
            Width = 150;
            Height = 75;
        }

        // For Serialization        
        public UMLClassViewModel() { }
    }
}
