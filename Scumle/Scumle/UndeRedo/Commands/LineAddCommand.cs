using Scumle.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.UndeRedo.Commands
{
    class LineAddCommand : UndoRedoCommand
    {
        Collection<LineViewModel> lines;
        LineViewModel line;
        public LineAddCommand(Collection<LineViewModel> _lines, LineViewModel _line)
        {
            lines = _lines;
            line = _line;
        }

        public override void Redo()
        {
            lines.Add(line);
        }

        public override void Undo()
        {
            lines.Remove(line);
        }
    }
}
