using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.UndeRedo
{
    /// <summary>
    /// Abstract class. Inherit this class and implement the Undo() and Redo()
    /// methods for a concrete implementation. The Execute() method can be called to
    /// add the command to the controller and execute the command immediately. Otherwise
    /// the add command can be called to only add the command the to controller.
    /// </summary>
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

        public void Add()
        {
            UndoRedoController.Instance.Add(this);
        }
    }
}
