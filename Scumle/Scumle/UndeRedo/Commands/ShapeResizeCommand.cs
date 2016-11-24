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
    public class ShapeResizeCommand : UndoRedoCommand
    {
        IShapeViewModel shape;
        Point oldpos;
        Point newpos;
        Size oldsize;
        Size newsize;

        public ShapeResizeCommand(IShapeViewModel _shape, Point _oldpos, Point _newpos, Size _oldsize, Size _newsize)
        {
            shape = _shape;
            oldpos = _oldpos;
            newpos = _newpos;
            oldsize = _oldsize;
            newsize = _newsize;

        }

        public override void Redo()
        {
            shape.X = newpos.X;
            shape.Y = newpos.Y;
            shape.Width = newsize.Width;
            shape.Height = newsize.Height;
        }

        public override void Undo()
        {
            shape.X = oldpos.X;
            shape.Y = oldpos.Y;
            shape.Width = oldsize.Width;
            shape.Height = oldsize.Height;
        }
    }
}
