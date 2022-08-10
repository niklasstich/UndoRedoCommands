using UndoRedoCommands.Data;

namespace UndoRedoCommands.Commands;

public sealed class UpdateModelCommand : UndoCommandBase
{
    private UselessModel _param;
    private UselessModel _model;

    public UpdateModelCommand(UselessModel param, UselessModel model)
    {
        _param = param;
        _model = model;
        CanExecute = true;
        CanRedo = false;
        CanUndo = false;
    }

    private void ExecuteInner()
    {
        var newParam = (UselessModel)_model.Clone();
        
        _model.Number = _param.Number;
        _model.Text = _param.Text;
        _model.IsTrue = _param.IsTrue;

        _param = newParam;
    }

    public override void Execute()
    {
        ExecuteInner();
        CanExecute = false;
        CanRedo = false;
        CanUndo = true;
    }

    public override void Undo()
    {
        ExecuteInner();
        CanExecute = false;
        CanRedo = true;
        CanUndo = false;
    }

    public override void Redo()
    {
        ExecuteInner();
        CanExecute = false;
        CanRedo = false;
        CanUndo = true;
    }
}