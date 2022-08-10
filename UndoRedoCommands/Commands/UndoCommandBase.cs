namespace UndoRedoCommands.Commands;

public abstract class UndoCommandBase : CommandBase, IUndoCommand
{
    protected UndoCommandBase()
    {
        _canUndo = false;
        _canRedo = false;
    }
    
    private bool _canUndo;
    private bool _canRedo;

    public virtual bool CanUndo
    {
        get => _canUndo;
        protected set => SetField(ref _canUndo, value);
    }

    public virtual bool CanRedo
    {
        get => _canRedo;
        protected set => SetField(ref _canRedo, value);
    }

    public abstract void Undo();
    public abstract void Redo();
}