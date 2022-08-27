namespace OriginatorGenerator;

public class MementoExcludeAttributeHelper
{
    public static string AttributeName => "MementoExclude";
    public static string Namespace => "OriginatorGenerator";
    public static string AttributeFullName => $"{Namespace}.{AttributeName}";
    public static string AttributeCode => @$"namespace {Namespace}
{{
    [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Field)]
    public class {AttributeName} : System.Attribute
    {{
    }}
}}";
}