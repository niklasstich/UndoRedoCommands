using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UndoRedoCommands.Commands;

public abstract class CommandBase : ICommand
{
    private bool _canExecute;

    protected CommandBase()
    {
        _canExecute = false;
    }

    protected CommandBase(bool canExecute)
    {
        _canExecute = canExecute;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
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

    public abstract void Execute();

    public virtual bool CanExecute
    {
        get => _canExecute;
        protected set => SetField(ref _canExecute, value);
    }
}