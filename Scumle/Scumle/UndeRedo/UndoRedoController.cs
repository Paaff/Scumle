using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.UndeRedo
{
    public class UndoRedoController
    {
        public static UndoRedoController Instance { get; } = new UndoRedoController();

        private readonly Stack<IUndoRedoCommand> undoStack = new Stack<IUndoRedoCommand>();
        private readonly Stack<IUndoRedoCommand> redoStack = new Stack<IUndoRedoCommand>();

        private UndoRedoController() { }

        public bool CanUndo() => undoStack.Count() > 0;

        public bool CanRedo() => redoStack.Count() > 0;

        public void Add(IUndoRedoCommand command)
        {
            undoStack.Push(command);
            redoStack.Clear();
        }

        public void Undo()
        {
            if (!CanUndo()) throw new InvalidOperationException();
            IUndoRedoCommand command = undoStack.Pop();
            redoStack.Push(command);
            command.Undo();
        }

        public void Redo()
        {
            if (!CanRedo()) throw new InvalidOperationException();
            IUndoRedoCommand command = redoStack.Pop();
            undoStack.Push(command);
            command.Redo();
        }

    }
}
