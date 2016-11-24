using Scumle.Model;
using Scumle.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Scumle.UndeRedo.Commands
{
    class ShapeColorCommand : UndoRedoCommand
    {
        List<IShapeViewModel> shapes;
        Dictionary<IShapeViewModel, Brush> old_colors;
        Brush newcolor;
        public ShapeColorCommand(List<IShapeViewModel> _shapes, Brush _color)
        {
            shapes = _shapes.ToList();
            newcolor = _color;
            old_colors = new Dictionary<IShapeViewModel, Brush>();

            foreach (IShapeViewModel shape in shapes)
            {
                old_colors.Add(shape, shape.ShapeColor);
            }
        }
        public override void Redo()
        {
            foreach (IShapeViewModel shape in shapes)
            {
                shape.ShapeColor = newcolor;
            }
        }

        public override void Undo()
        {
            foreach (IShapeViewModel shape in shapes)
            {
                Brush old_color;
                if (old_colors.TryGetValue(shape, out old_color))
                {
                    shape.ShapeColor = old_color;
                }
            }
        }
    }
}
