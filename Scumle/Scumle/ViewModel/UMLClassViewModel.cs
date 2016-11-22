using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scumle.Model;
using System.Windows.Media;

namespace Scumle.ViewModel
{
    public class UMLClassViewModel : ShapeViewModel
    {
        public UMLClassViewModel(Shape shape) : base(shape)
        {
            Width = 300;
            Height = 150;
            ShapeColor = new SolidColorBrush(Color.FromRgb(232, 232, 232));
            
        }
               
    }
}
