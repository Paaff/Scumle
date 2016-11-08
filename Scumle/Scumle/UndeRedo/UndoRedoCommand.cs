using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.UndeRedo
{
    public abstract class UndoRedoCommand
    {
        public abstract void Undo();

        public abstract void Redo();

        public void Execute()
        {
            UndoRedoController.Instance.Add(this);
            this.Redo();
            UndoRedoController.Instance.UpdateCommandStatus();
        }
    }
}
