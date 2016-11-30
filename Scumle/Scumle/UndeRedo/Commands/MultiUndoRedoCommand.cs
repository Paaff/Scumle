using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.UndeRedo.Commands
{
    class MultiUndoRedoCommand : UndoRedoCommand
    {
        IList<UndoRedoCommand> commands;
        public MultiUndoRedoCommand(IList<UndoRedoCommand> _commands)
        {
            commands = _commands.ToList();
        }

        public MultiUndoRedoCommand(params UndoRedoCommand[] _commands)
        {
            commands = _commands.ToList();
        }

        public override void Redo()
        {
            foreach(UndoRedoCommand command in commands)
            {
                command.Redo();
            }
        }

        public override void Undo()
        {
            foreach (UndoRedoCommand command in commands)
            {
                command.Undo();
            }
        }
    }
}
