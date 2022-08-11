using System.ComponentModel;

namespace UndoRedoCommands.Commands;

public interface ICommand
{
    public void Execute();
}