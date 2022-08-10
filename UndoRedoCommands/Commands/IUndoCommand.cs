namespace UndoRedoCommands.Commands;

public interface IUndoCommand : ICommand
{
    public bool CanUndo { get; }
    public bool CanRedo { get; }
    public void Undo();
    public void Redo();
}