using System.ComponentModel;

namespace UndoRedoCommands.Commands;

public interface ICommandStateManager : INotifyPropertyChanged
{
    public bool CanUndo { get; }
    public bool CanRedo { get; }
    public void Execute(ICommand command);
    public void Undo();
    public void Redo();
}