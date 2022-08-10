using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UndoRedoCommands.Commands;

public class CommandStateManager : ICommandStateManager
{
    private Stack<IUndoCommand> _undo;
    private Stack<IUndoCommand> _redo;

    public CommandStateManager()
    {
        _undo = new Stack<IUndoCommand>();
        _redo = new Stack<IUndoCommand>();
    }

    public bool CanUndo => _undo.Any();
    public bool CanRedo => _redo.Any();
    public void Execute(ICommand command)
    {
        command.Execute();
        if (command is IUndoCommand undoCommand)
        {
            _undo.Push(undoCommand);
        }
    }

    public void Undo()
    {
        if (!CanUndo) return;
        var command = _undo.Pop();
        command.Undo();
        _redo.Push(command);
    }

    public void Redo()
    {
        if (!CanRedo) return;
        var command = _redo.Pop();
        command.Redo();
        _undo.Push(command);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}