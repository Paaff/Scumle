using Scumle.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Scumle.ViewModel
{
    public interface IShapeViewModel
    {
        Shape Shape { get; }
        double X { get; set; }
        double Y { get; set; }
        double Height { get; set; }
        double Width { get; set; }
        Brush ShapeColor { get; set; }
        bool IsSelected { get; set; }
        ObservableCollection<ConnectionPointViewModel> ConnectionPoints { get; } 
        void ShapeResize(double dX, double dY);
        void ShapeMove(double dX, double dY);
    }
}
