using PostSharp.Patterns.Recording;

namespace UndoRedoCommands.Data;

[Recordable]
public class UselessModel : IOriginator
{
    public UselessModel()
    {
        Number = 0;
        IsTrue = false;
        Text = "";
    }
    public int Number { get; set; }
    public bool IsTrue { get; set; }
    public string Text { get; set; }
    public IMemento GetMemento()
    {
        return new UselessMemento(Number, IsTrue, Text);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not UselessMemento uselessMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }
        Number = uselessMemento.Number;
        IsTrue = uselessMemento.IsTrue;
        Text = uselessMemento.Text;
    }

    private record UselessMemento : IMemento
    {
        internal UselessMemento(int number, bool isTrue, string text)
        {
            Number = number;
            IsTrue = isTrue;
            Text = text;
        }

        internal int Number { get; }
        internal bool IsTrue { get; }
        internal string Text { get; }
    }

}