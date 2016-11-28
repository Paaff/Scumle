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
    /// <summary>
    /// Provides a dummy collection of shapes to be able to present to the user
    /// in the view for selection.
    /// </summary>
    public static class ShapesPreview
    {

        public static ObservableCollection<IShape> List = new ObservableCollection<IShape>()
        {
            new BasicShapeViewModel(new BasicShape(EBasicShape.Ellipse, 50, 50, 50,50, Color.FromRgb(205,92,92), "previewID")),
            new UMLClassViewModel(new UMLClass(0, 0,50,50, "UML Class",Color.FromRgb(205,92,92),"previewID", "-First field : int\n-Second Field : String", "+First method()\n+Second method()")),
            new BasicShapeViewModel(new BasicShape(EBasicShape.Rectangle, 0, 0, 50,50,Color.FromRgb(205,92,92), "previewID"))
        };
    }
}
