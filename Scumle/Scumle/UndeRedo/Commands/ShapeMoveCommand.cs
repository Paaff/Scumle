using Scumle.Model;
using Scumle.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Scumle.UndeRedo.Commands
{
    /// <summary>
    /// Move one or more shapes on the diagram
    /// </summary>
    class ShapeMoveCommand : UndoRedoCommand
    {
        IList<IShape> shapes;
        double offsetX;
        double offsetY;

        public ShapeMoveCommand(IList<IShape> _shapes, double _offsetX, double _offsetY)
        {
            shapes = _shapes.ToList();
            offsetX = _offsetX;
            offsetY = _offsetY;
        }

        public override void Redo()
        {
            foreach (IShape shape in shapes)
            {
                shape.X += offsetX;
                shape.Y += offsetY;
            }
        }

        public override void Undo()
        {
            foreach (IShape shape in shapes)
            {
                shape.X -= offsetX;
                shape.Y -= offsetY;
            }
        }
    }
}
