namespace UndoRedoCommands.Commands;

public interface IUndoCommand : ICommand
{
    public void Undo();
    public void Redo();
}