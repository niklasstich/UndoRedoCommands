using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace OriginatorGenerator
{
    [Generator]
    public class OriginatorGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                ctx.AddSource("Test123Attribute.g.cs",
                    SourceText.From(Test123Helper.AttributeCode, Encoding.UTF8));
                ctx.AddSource("IOriginator.g.cs",
                    SourceText.From(IOriginatorHelper.InterfaceCode, Encoding.UTF8));
                ctx.AddSource("IMemento.g.cs",
                    SourceText.From(IMementoHelper.InterfaceCode, Encoding.UTF8));
            });
            
            
            var classDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: IsTargetForGeneration,
                    transform: GetSemanticTargetForGeneration)
                .Where(static v => v is not null);

            var compilationAndClasses =
                context.CompilationProvider.Combine(classDeclarations.Collect());
            
            context.RegisterSourceOutput(compilationAndClasses, static (spc, source) => Execute(source.Left, source.Right, spc));
        }

        private static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax?> classes, SourceProductionContext context)
        {
            if (classes.IsDefaultOrEmpty) return;
            var distinctClasses = classes.Distinct();
            
            foreach (var classDeclaration in distinctClasses)
            {
                if (classDeclaration is null)
                {
                    continue;
                }

                BuildIMementoImplementation(compilation, context, classDeclaration);
            }
        }

        private static void BuildIMementoImplementation(Compilation compilation, SourceProductionContext context,
            ClassDeclarationSyntax classDeclaration)
        {
            var sb = new StringBuilder();
            var identifierText = classDeclaration.Identifier.Text;
            var nameSpace = GetNamespace(classDeclaration);
            var semanticModel = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
            if (semanticModel.GetDeclaredSymbol(classDeclaration) is not { } typeSymbol)
                throw new Exception("type wasn't INamedTypeSymbol");
            var members = typeSymbol.GetMembers();
            var properties = members.OfType<IPropertySymbol>();
            var fields = members.OfType<IFieldSymbol>();
            sb.AppendLine($"using {IOriginatorHelper.InterfaceNamespace};");
            sb.AppendLine();
            sb.AppendLine($"namespace {nameSpace};");
            sb.AppendLine();
            sb.AppendLine($@"public partial class {identifierText} : {IOriginatorHelper.InterfaceName}
{{
    public IMemento CreateMemento()
    {{
        return new ConcreteMemento(this);
    }}

    public void RestoreMemento(IMemento memento)
    {{
        //restore from memento here
        throw new NotImplementedException();
    }}
");
            sb.AppendLine(@$"
    private class ConcreteMemento : {IMementoHelper.InterfaceName}
    {{
        public ConcreteMemento({identifierText} originator) {{
            //constructor here
        }}");
            foreach (var property in properties)
            {
                if (property.SetMethod is not null)
                {
                    if (!property.SetMethod.IsImplicitlyDeclared)
                    {
                        
                    }
                    sb.AppendLine($"        public {property.Type.ToDisplayString()} {property.Name} {{ get; set; }}");
                }
                break;
            }

            foreach (var field in fields)
            {
                if(!field.IsImplicitlyDeclared)
                    sb.AppendLine($"        public {field.Type.ToDisplayString()} {field.Name};");
                break;
            }

            sb.Append(@"
    }
}");
            
            context.AddSource($"{identifierText}.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }

        private static bool IsTargetForGeneration(SyntaxNode node, CancellationToken _) =>
            node is ClassDeclarationSyntax { AttributeLists.Count: > 0 };

        private static ClassDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context, CancellationToken cancellationToken)
        {
            var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

            foreach (var attributeListSyntax in classDeclarationSyntax.AttributeLists)
            {
                foreach (var attributeSyntax in attributeListSyntax.Attributes)
                {
                    if (cancellationToken.IsCancellationRequested) return null;
                    var attributeSymbol = ModelExtensions.GetSymbolInfo(context.SemanticModel, attributeSyntax, cancellationToken);
                    var attributeContainingTypeSymbol = attributeSymbol.Symbol?.ContainingType;
                    var fullName = attributeContainingTypeSymbol?.ToDisplayString();
                    Debugger.Launch();
                    if (fullName != Test123Helper.AttributeFullName) continue; 
                    return classDeclarationSyntax.Modifiers.Any(SyntaxKind.PartialKeyword) ? classDeclarationSyntax : null;
                }
            }

            return null;
        }
        
        private static string GetNamespace(BaseTypeDeclarationSyntax syntax)
        {
            //iterate through parents until we run find namespace syntax element
            var potentialNamespaceParent = syntax.Parent;
            while (potentialNamespaceParent != null
                   && potentialNamespaceParent is not NamespaceDeclarationSyntax
                   && potentialNamespaceParent is not FileScopedNamespaceDeclarationSyntax)
            {
                potentialNamespaceParent = potentialNamespaceParent.Parent;
            }

            if (potentialNamespaceParent is not BaseNamespaceDeclarationSyntax namespaceParent) return string.Empty;
            
            var nameSpace = namespaceParent.Name.ToString();

            //handle nested namespaces
            while (true)
            {
                if (namespaceParent.Parent is not NamespaceDeclarationSyntax parent)
                    break;
                nameSpace = $"{namespaceParent.Name}.{nameSpace}";
                namespaceParent = parent;
            }

            return nameSpace;
        }
    }
}