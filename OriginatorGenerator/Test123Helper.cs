namespace OriginatorGenerator;

public static class Test123Helper
{
    public static string AttributeName => "Test123";
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