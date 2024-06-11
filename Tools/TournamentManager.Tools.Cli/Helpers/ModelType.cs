
using System.Reflection;
using System.Text;

namespace TournamentManager.Tools.Cli;

public class ModelType
{
    public static readonly string OutputFolder = @$"{SolutionHelper.GetMainDirectory().FullName}\Client\libs\tournament-manager-domain\src\models";
    private readonly Type _type;
    private readonly Dictionary<string, string> _import = [];
    private readonly StringBuilder _properties = new();
    private string _baseTypeName;

    public ModelType(Type type)
    {
        _type = type;
    }

    public void Generate()
    {
        var sb = new StringBuilder();
        sb.AddGeneratorInformation();
        if (_import.Any())
        {
            foreach (var import in _import)
            {
                sb.AppendLine($"import {{ {import.Value} }} from \"{import.Key}\";");
            }
            sb.AppendLine();
        }

        sb.Append($"export interface {_type.Name}");
        if (_baseTypeName != null)
        {
            sb.Append($" extends {_baseTypeName}");
        }
        sb.AppendLine("{");
        sb.Append(_properties.ToString());
        sb.AppendLine("}");

        File.WriteAllText(@$"{OutputFolder}\generated\{_type.Name.PascalToKebabCase()}.ts", sb.ToString());
    }

    public void Load()
    {
        SetBaseType();
        LoadProperties();
    }

    private void SetBaseType()
    {
        if (_type.BaseType?.Assembly.FullName.StartsWith("TournamentManager.") != true)
        {
            return;
        }

        _baseTypeName = _type.BaseType.Name;
        AddImport(_type.BaseType.Name.PascalToKebabCase(), _baseTypeName);
    }

    private void LoadProperties()
    {
        foreach (var property in _type.GetProperties())
        {
            if (!UseProperty(property))
            {
                continue;
            }

            var nullableText = property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                ? "?"
                : string.Empty;

            _properties.AppendLine($"    {property.Name.PascalToCamelCase()}{nullableText}: {GetTypescriptType(property.PropertyType)}");
        }
    }

    private void AddImport(string fileName, string modelName)
    {
        if (_import.ContainsKey(fileName))
        {
            return;
        }

        _import.Add($"./{fileName}", modelName);
    }

    private bool UseProperty(PropertyInfo property)
    {
        return property.GetMethod.IsPublic && !property.GetMethod.IsVirtual && property.DeclaringType == _type;
    }

    private string GetTypescriptType(Type propertyType)
    {
        if (propertyType.IsGenericType)
        {
            return propertyType.GetGenericTypeDefinition() switch {
                Type nullableType when nullableType == typeof(Nullable<>) => GetTypescriptType(propertyType.GenericTypeArguments[0]),
                Type collectionType when collectionType == typeof(List<>) => $"{GetTypescriptType(propertyType.GenericTypeArguments[0])}[]",
            };
        }

        if (propertyType.Assembly.FullName?.StartsWith("TournamentManager.") == true)
        {
            var enumPath = propertyType.IsEnum 
                ? "../../enums/generated/"
                : "./";
            AddImport($"{enumPath}{propertyType.Name.PascalToKebabCase()}", propertyType.Name);
            return propertyType.Name;
        }

        return propertyType switch {
            Type stringType when stringType == typeof(string) => "string",
            Type numberType when numberType == typeof(int) || numberType == typeof(short) || numberType == typeof(long) => "number",
            Type dateType when dateType == typeof(DateTime) => "Date",
            _ => throw new NotSupportedException($"Type {propertyType} is not supported")
        };
    }
}
