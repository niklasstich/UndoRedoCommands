using System.ComponentModel;
using UndoRedoCommands.Data;

namespace UndoRedoCommands.Commands;

public class UpdateModelCommand : IUndoCommand
{
    private UselessModel _param;
    private readonly UselessModel _model;

    public UpdateModelCommand(UselessModel param, UselessModel model)
    {
        _param = param;
        _model = model;
    }

    private void ExecuteInner()
    {
        var newParam = (UselessModel)_model.Clone();
        
        _model.Number = _param.Number;
        _model.Text = _param.Text;
        _model.IsTrue = _param.IsTrue;

        _param = newParam;
    }

    public void Execute() => ExecuteInner();

    public void Undo() => ExecuteInner();

    public void Redo() => ExecuteInner();
}