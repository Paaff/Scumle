using Scumle.Model;
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
        private ObservableCollection<IShape> shapes;
        private IShape shape;

        public ShapeAddCommand(ObservableCollection<IShape> _shapes, IShape _shape)
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
