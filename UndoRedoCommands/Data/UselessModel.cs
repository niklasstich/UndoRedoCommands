namespace UndoRedoCommands.Data;

public class UselessModel : ICloneable
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
    public object Clone()
    {
        return MemberwiseClone();
    }
}