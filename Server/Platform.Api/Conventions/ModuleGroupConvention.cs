using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Platform.Api.Conventions;

/// <summary>
/// Assigns each controller to a Swagger document group based on its namespace.
/// The group name is derived automatically from the namespace segment that
/// precedes ".Api" (e.g. "Design.Api.Controllers" → "design").
/// </summary>
public class ModuleGroupConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        var ns = controller.ControllerType.Namespace ?? string.Empty;

        // Find the segment directly before the first ".Api" occurrence.
        // E.g. "Design.Api.Controllers" → "Design" → "design"
        var apiMarker = ".Api";
        var markerIndex = ns.IndexOf(apiMarker, StringComparison.OrdinalIgnoreCase);
        if (markerIndex <= 0)
            return;

        var beforeMarker = ns[..markerIndex];
        var lastDot = beforeMarker.LastIndexOf('.');
        var moduleName = lastDot >= 0 ? beforeMarker[(lastDot + 1)..] : beforeMarker;

        controller.ApiExplorer.GroupName = moduleName.ToLowerInvariant();
    }
}
