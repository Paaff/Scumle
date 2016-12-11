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
    /// <summary>
    /// Add one or multiple shapes to the diagram
    /// </summary>
    public class ShapeAddCommand : UndoRedoCommand
    {
        private ObservableCollection<IShape> shapes;
        private IList<IShape> add_shapes;

        public ShapeAddCommand(ObservableCollection<IShape> _shapes, IShape _shape)
        {
            shapes = _shapes;
            add_shapes = new List<IShape>() { _shape };
        }

        public ShapeAddCommand(ObservableCollection<IShape> _shapes, IList<IShape> _add_shapes)
        {
            shapes = _shapes;
            add_shapes = _add_shapes.ToList();
        }

        public override void Undo()
        {
            foreach(IShape shape in add_shapes)
            {
                shapes.Remove(shape);
                shape.IsSelected = false;
            }
        }

        public override void Redo()
        {
            foreach (IShape shape in add_shapes)
            {
                shapes.Add(shape);
            }
        }
    }
}
