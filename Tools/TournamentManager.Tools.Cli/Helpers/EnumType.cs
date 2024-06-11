
using System.Text;

namespace TournamentManager.Tools.Cli;

public class EnumType
{
    public static readonly string OutputFolder = @$"{SolutionHelper.GetMainDirectory().FullName}\Client\libs\tournament-manager-domain\src\enums";
    private readonly Type _type;
    private readonly StringBuilder _values = new();

    public EnumType(Type type)
    {
        _type = type;
    }

    public void Generate()
    {
        var sb = new StringBuilder();
        sb.AddGeneratorInformation();
        sb.AppendLine($"export enum {_type.Name} {{");
        sb.Append(_values.ToString());
        sb.AppendLine("}");

        File.WriteAllText(@$"{OutputFolder}\generated\{_type.Name.PascalToKebabCase()}.ts", sb.ToString());
    }

    public void Load()
    {
        var underlyingEnumType = Enum.GetUnderlyingType(_type);
        foreach (var value in Enum.GetValues(_type))
        {
            _values.AppendLine($"    {value.ToString()} = {Convert.ChangeType(value, underlyingEnumType)},");
        }
    }
}
