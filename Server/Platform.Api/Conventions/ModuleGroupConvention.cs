using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Platform.Api.Conventions;

/// <summary>
/// Assigns each controller to a Swagger document group based on its namespace.
/// Controllers in "Design.Api.*" are grouped under "design",
/// controllers in "Competition.Api.*" are grouped under "competition", etc.
/// </summary>
public class ModuleGroupConvention : IControllerModelConvention
{
    private static readonly Dictionary<string, string> _namespacePrefixToGroup = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Design.Api"] = "design",
        ["Competition.Api"] = "competition",
    };

    public void Apply(ControllerModel controller)
    {
        var controllerNamespace = controller.ControllerType.Namespace ?? string.Empty;

        foreach (var (prefix, group) in _namespacePrefixToGroup)
        {
            if (controllerNamespace.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                controller.ApiExplorer.GroupName = group;
                return;
            }
        }
    }
}
