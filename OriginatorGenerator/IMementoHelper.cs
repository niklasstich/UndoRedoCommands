namespace OriginatorGenerator;

public class IMementoHelper
{
    public static string InterfaceName => "IMemento";
    public static string InterfaceNamespace => "OriginatorGenerator";
    public static string InterfaceFullName => $"{InterfaceNamespace}.{InterfaceName}";

    public static string InterfaceCode => $@"namespace {InterfaceNamespace};

public interface {InterfaceName} 
{{
    
}}";
}