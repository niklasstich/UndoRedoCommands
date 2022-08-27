using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace OriginatorGenerator;

[Generator]
public class TestGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var code = @"
using System;
namespace Hello {
    public static class World 
    {
        public const string Name = ""Khalid"";
        public static void Hi() => Console.WriteLine($""Hi, {Name}!"");
    }
}";
        context.AddSource("helloworld.generated.cs", SourceText.From(code, Encoding.UTF8));
    }
}