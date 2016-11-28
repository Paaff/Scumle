using GalaSoft.MvvmLight.Command;
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

        private readonly Stack<UndoRedoCommand> undoStack = new Stack<UndoRedoCommand>();
        private readonly Stack<UndoRedoCommand> redoStack = new Stack<UndoRedoCommand>();

        public RelayCommand UndoCommand;
        public RelayCommand RedoCommand;


        internal void UpdateCommandStatus()
        {
            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();
        }

        private UndoRedoController() {
            UndoCommand = new RelayCommand(Undo, CanUndo);
            RedoCommand = new RelayCommand(Redo, CanRedo);
        }

        public bool CanUndo() => undoStack.Count() > 0;

        public bool CanRedo() => redoStack.Count() > 0;

        public void Add(UndoRedoCommand command)
        {
            undoStack.Push(command);
            redoStack.Clear();
            UpdateCommandStatus();
        }

        public void Undo()
        {
            if (!CanUndo()) throw new InvalidOperationException();
            UndoRedoCommand command = undoStack.Pop();
            redoStack.Push(command);
            command.Undo();
            UpdateCommandStatus();
        }

        public void Redo()
        {
            if (!CanRedo()) throw new InvalidOperationException();
            UndoRedoCommand command = redoStack.Pop();
            undoStack.Push(command);
            command.Redo();
            UpdateCommandStatus();
        }

    }
}
