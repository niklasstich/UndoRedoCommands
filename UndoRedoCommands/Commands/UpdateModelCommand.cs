using PostSharp.Patterns.Recording;
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
        using var scope = RecordingServices.DefaultRecorder.OpenScope("Update model");
        _model.Number = _param.Number;
        _model.Text = _param.Text;
        _model.IsTrue = _param.IsTrue;
    }

    public void Undo()
    {
        var operations = RecordingServices.DefaultRecorder.UndoOperations;
        RecordingServices.DefaultRecorder.Undo();
    }

    public void Redo()
    {
        RecordingServices.DefaultRecorder.Redo();
    }
}