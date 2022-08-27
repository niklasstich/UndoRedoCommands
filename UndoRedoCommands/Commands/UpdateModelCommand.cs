using UndoRedoCommands.Data;

namespace UndoRedoCommands.Commands;

public class UpdateModelCommand : IUndoCommand
{
    private UselessModel _param;
    private readonly UselessModel _model;
    private IMemento? _memento;

    public UpdateModelCommand(UselessModel param, UselessModel model)
    {
        _param = param;
        _model = model;
    }

    public void Execute()
    {
        _memento = _model.GetMemento();
        
        _model.Number = _param.Number;
        _model.Text = _param.Text;
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        _model.RestoreMemento(_memento);
    }

    public void Redo() => Execute();
}