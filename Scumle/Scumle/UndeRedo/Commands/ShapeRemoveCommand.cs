using Scumle.Model;
using Scumle.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.UndeRedo.Commands
{
    class ShapeRemoveCommand : UndoRedoCommand
    {
        Collection<IShape> shapes;
        Collection<LineViewModel> lines;
        List<IShape> remove_shapes;
        List<LineViewModel> remove_lines;
        public ShapeRemoveCommand(Collection<IShape> _shapes, Collection<LineViewModel> _lines, List<IShape> _remove)
        {
            shapes = _shapes;
            lines = _lines;
            remove_shapes = _remove.ToList();
            remove_lines = lines.Where(l => remove_shapes.Any(s => l.From.Shape == s || l.To.Shape == s)).ToList();
        }

        public override void Undo()
        {
            remove_shapes.ForEach(s => shapes.Add(s));
            remove_lines.ForEach(l => lines.Add(l));
        }

        public override void Redo()
        {
            remove_shapes.ForEach(s => s.IsSelected = false);
            remove_shapes.ForEach(s => shapes.Remove(s));
            remove_lines.ForEach(l => lines.Remove(l));
        }
    }
}
