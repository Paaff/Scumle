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
    class LineAddCommand : UndoRedoCommand
    {
        ObservableCollection<ILine> lines;
        IList<ILine> add_lines;
        public LineAddCommand(ObservableCollection<ILine> _lines, ILine _line)
        {
            lines = _lines;
            add_lines = new List<ILine>() { _line }; 
        }

        public LineAddCommand(ObservableCollection<ILine> _lines, IList<ILine> _add_lines)
        {
            lines = _lines;
            add_lines = _add_lines.ToList();
        }

        public override void Undo()
        {
            foreach (ILine line in add_lines)
            {
                lines.Remove(line);
            }
        }

        public override void Redo()
        {
            foreach (ILine line in add_lines)
            {
                lines.Add(line);
                (line as LineViewModel)?.UpdateProperties(this, EventArgs.Empty);
            }
        }
    }
}
