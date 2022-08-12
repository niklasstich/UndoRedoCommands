namespace UndoRedoCommands.Data;

public interface IOriginator
{
    IMemento GetMemento();
    void RestoreMemento(IMemento memento);
}