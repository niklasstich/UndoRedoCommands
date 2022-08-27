using OriginatorGenerator;

namespace OriginatorGeneratorConsumer;

[Test123]
public partial class Foobarbaz
{
    public string Name { get; set; }
    
}

[Test123]
public partial class Foo
{
    private string _name;

    public string Name
    {
        get => _name;
        set => _name = value;
    }
}

[Test123]
public partial class Baz
{
    private int Number { get; set; }
}