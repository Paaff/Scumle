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

namespace Scumle.View.Preview
{

    public static class LinesPreview
    {
        private static IShape DummyShape = new BasicShapeViewModel(new BasicShape(EBasicShape.Ellipse, 0, 0));
        private static PreviewConnectionPointViewModel DummyFrom = new PreviewConnectionPointViewModel(DummyShape, 0, 0);
        private static PreviewConnectionPointViewModel DummyTo = new PreviewConnectionPointViewModel(DummyShape, 50, 50);

        private static ConnectionPointViewModel DummyFrom2 = new ConnectionPointViewModel(new PreviewConnectionPoint(DummyShape.Shape, 0, 0), DummyShape);
        private static ConnectionPointViewModel DummyTo2 = new ConnectionPointViewModel(new PreviewConnectionPoint(DummyShape.Shape, 50, 50), DummyShape);
        public static ObservableCollection<LineViewModel> List = new ObservableCollection<LineViewModel>()
        {
            new LineViewModel(ELine.Association, DummyFrom2, DummyTo2),
            new LineViewModel(ELine.Inheritance, DummyFrom2, DummyTo2)
        };
    }
}
