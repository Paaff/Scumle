using Scumle.Model;
using Scumle.Model.Shapes;
using Scumle.ViewModel;
using Scumle.ViewModel.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Scumle.View.Preview
{
    public static class LinesPreview
    {
        private static BasicShapeViewModel Shape = new BasicShapeViewModel(new BasicShape(Model.EBasicShape.Ellipse, 0, 0, Color.FromRgb(205,92,92), "previewID"));
        private static ConnectionPointViewModel From = new ConnectionPointViewModel(new ConnectionPoint(Shape.Shape, System.Windows.HorizontalAlignment.Left, System.Windows.VerticalAlignment.Top), Shape);
        private static ConnectionPointViewModel To = new ConnectionPointViewModel(new ConnectionPoint(Shape.Shape, System.Windows.HorizontalAlignment.Right, System.Windows.VerticalAlignment.Bottom), Shape);

        public static ObservableCollection<LineViewModel> List = new ObservableCollection<LineViewModel>()
        {
            new LineViewModel(new Line(ELine.Association, From, To)),
            new LineViewModel(new Line(ELine.Inheritance, From, To)),
            new LineViewModel(new Line(ELine.Relational, From, To))
        };

    }
}
