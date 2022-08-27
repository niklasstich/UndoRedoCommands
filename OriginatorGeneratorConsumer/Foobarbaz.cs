using OriginatorGenerator;

namespace OriginatorGeneratorConsumer;

[Memento]
public partial class Foobarbaz
{
    public string Name { get; set; }
    
}

[Memento]
public partial class Foo
{
    [MementoExclude]
    private string _name;

    public string Name
    {
        get => _name;
        set => _name = value;
    }
}

[Memento]
public partial class Baz
{
    private int Number { get; set; }
}