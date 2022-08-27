namespace OriginatorGenerator;

public static class MementoAttributeHelper
{
    public static string AttributeName => "Memento";
    public static string Namespace => "OriginatorGenerator";
    public static string AttributeFullName => $"{Namespace}.{AttributeName}";
    public static string AttributeCode => @$"namespace {Namespace}
{{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class {AttributeName} : System.Attribute
    {{
    }}
}}";
}