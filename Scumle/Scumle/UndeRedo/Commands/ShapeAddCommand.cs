using Scumle.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.UndeRedo
{
    public class ShapeAddCommand : UndoRedoCommand
    {
        private ObservableCollection<ShapeViewModel> shapes;
        private ShapeViewModel shape;

        public ShapeAddCommand(ObservableCollection<ShapeViewModel> _shapes, ShapeViewModel _shape)
        {
            shapes = _shapes;
            shape = _shape;
        }

        public override void Undo()
        {
            shape.IsSelected = false;
            shapes.Remove(shape);
        }

        public override void Redo()
        {
            shapes.Add(shape);
        }
    }
}
