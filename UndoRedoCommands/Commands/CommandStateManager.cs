using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UndoRedoCommands.Commands;

public sealed class CommandStateManager : ICommandStateManager
{
    private readonly Stack<IUndoCommand> _undo;
    private readonly Stack<IUndoCommand> _redo;

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
        _redo.Clear();
    }

    public void Undo()
    {
        if (!CanUndo) return;
        var command = PopUndo();
        command.Undo();
        PushRedo(command);
    }

    public void Redo()
    {
        if (!CanRedo) return;
        var command = PopRedo();
        command.Redo();
        PushUndo(command);
    }

    private IUndoCommand PopUndo()
    {
        var canUndoBefore = CanUndo;
        var command = _undo.Pop();
        if (canUndoBefore != CanUndo)
        {
            OnPropertyChanged(nameof(CanUndo));
        }

        return command;
    }
    
    private IUndoCommand PopRedo()
    {
        var canRedoBefore = CanRedo;
        var command = _redo.Pop();
        if (canRedoBefore != CanRedo)
        {
            OnPropertyChanged(nameof(CanRedo));
        }

        return command;
    }
    
    private void PushUndo(IUndoCommand command)
    {
        var canUndoBefore = CanUndo;
        _undo.Push(command);
        if (canUndoBefore != CanUndo)
        {
            OnPropertyChanged(nameof(CanUndo));
        }
    }

    private void PushRedo(IUndoCommand command)
    {
        var canRedoBefore = CanRedo;
        _redo.Push(command);
        if (canRedoBefore != CanRedo)
        {
            OnPropertyChanged(nameof(CanRedo));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}