using Scumle.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.UndeRedo
{
    public class AddShapeUndoRedo : IUndoRedoCommand
    {
        private ObservableCollection<ShapeViewModel> shapes;
        private ShapeViewModel shape;

        public AddShapeUndoRedo(ObservableCollection<ShapeViewModel> _shapes, ShapeViewModel _shape)
        {
            shapes = _shapes;
            shape = _shape;

        }
        public void Redo()
        {
            shapes.Add(shape);
        }

        public void Undo()
        {
            shapes.Remove(shape);
        }
    }
}
