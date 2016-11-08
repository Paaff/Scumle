using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.UndeRedo
{
    public interface IUndoRedoCommand
    {
        void Undo();

        void Redo();
    }
}
