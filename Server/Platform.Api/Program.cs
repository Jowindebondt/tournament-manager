using Competition.Infrastructure.Extensions;
using Design.Infrastructure.Extensions;
using Generation.Infrastructure.Extensions;
using Microsoft.OpenApi.Models;
using Platform.Api.Conventions;

var builder = WebApplication.CreateBuilder(args);

// ── Module registrations ──────────────────────────────────────────────────────
// Each module owns its own DI wiring (DbContext, repositories, MediatR handlers,
// AutoMapper profiles, and its public facade). When a module is extracted into a
// microservice its host project calls the same extension (or an equivalent HTTP
// client variant) during startup.

builder.Services.AddDesignModule(builder.Configuration);
builder.Services.AddCompetitionModule(builder.Configuration);
builder.Services.AddGenerationModule();

// ── MVC + module group convention ────────────────────────────────────────────

builder.Services.AddControllers(options =>
    options.Conventions.Add(new ModuleGroupConvention()));

// ── AutoMapper – Api-layer profiles (ViewModel ↔ DTO) ────────────────────────
// Application-layer profiles are registered inside each module extension above.

builder.Services.AddAutoMapper(
    typeof(Design.Api.MappingProfile),
    typeof(Competition.Api.MappingProfile));

// ── Swagger – one document per module ─────────────────────────────────────────

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("design", new OpenApiInfo
    {
        Title = "Design Module",
        Version = "v1",
        Description = "API for managing tournament designs (tournaments, rounds, poules)."
    });

    options.SwaggerDoc("competition", new OpenApiInfo
    {
        Title = "Competition Module",
        Version = "v1",
        Description = "API for managing live competitions (matches, results)."
    });

    options.SwaggerDoc("generation", new OpenApiInfo
    {
        Title = "Generation Module",
        Version = "v1",
        Description = "API for generating competitions from tournament designs."
    });

    options.SwaggerDoc("tabletennis", new OpenApiInfo
    {
        Title = "Sports.TableTennis Module",
        Version = "v1",
        Description = "API for Table Tennis sport-specific settings."
    });

    // Only include operations that belong to the current document's group
    options.DocInclusionPredicate((docName, apiDesc) =>
    {
        if (apiDesc.ActionDescriptor is not Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor actionDesc)
            return false;

        var groupName = actionDesc.ControllerTypeInfo.GetCustomAttributes(
            typeof(Microsoft.AspNetCore.Mvc.ApiExplorerSettingsAttribute), inherit: true)
            .OfType<Microsoft.AspNetCore.Mvc.ApiExplorerSettingsAttribute>()
            .FirstOrDefault()?.GroupName;

        // Fall back to the ApiExplorer group set by the convention
        if (groupName == null && apiDesc.GroupName != null)
            groupName = apiDesc.GroupName;

        return string.Equals(groupName, docName, StringComparison.OrdinalIgnoreCase);
    });
});

// ─────────────────────────────────────────────────────────────────────────────

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/design/swagger.json", "Design Module");
    options.SwaggerEndpoint("/swagger/competition/swagger.json", "Competition Module");
    options.SwaggerEndpoint("/swagger/generation/swagger.json", "Generation Module");
    options.SwaggerEndpoint("/swagger/tabletennis/swagger.json", "Sports.TableTennis Module");
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
