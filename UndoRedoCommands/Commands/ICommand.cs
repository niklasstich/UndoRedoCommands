using System.ComponentModel;

namespace UndoRedoCommands.Commands;

public interface ICommand : INotifyPropertyChanged
{
    public void Execute();
    public bool CanExecute { get; }
}