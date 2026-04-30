using Competition.Domain.Interfaces;
using Competition.Infrastructure.Persistence;
using Competition.Infrastructure.Repositories;
using Design.Domain.Interfaces;
using Design.Infrastructure.Persistence;
using Design.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Platform.Api.Conventions;

var builder = WebApplication.CreateBuilder(args);

// ── Database contexts ──────────────────────────────────────────────────────────

var designConnectionString = builder.Configuration.GetConnectionString("DesignConnection");
builder.Services.AddDbContext<DesignDbContext>(options =>
    options.UseSqlServer(designConnectionString));

var competitionConnectionString = builder.Configuration.GetConnectionString("CompetitionConnection");
builder.Services.AddDbContext<CompetitionDbContext>(options =>
    options.UseSqlServer(competitionConnectionString));

// ── MVC + module group convention ────────────────────────────────────────────

builder.Services.AddControllers(options =>
    options.Conventions.Add(new ModuleGroupConvention()));

// ── MediatR (all module application assemblies) ───────────────────────────────

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Design.Application.MappingProfile).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(Competition.Application.MappingProfile).Assembly);
});

// ── AutoMapper (all module mapping profiles) ──────────────────────────────────

builder.Services.AddAutoMapper(
    typeof(Design.Api.MappingProfile),
    typeof(Design.Application.MappingProfile),
    typeof(Competition.Api.MappingProfile),
    typeof(Competition.Application.MappingProfile));

// ── Repositories ──────────────────────────────────────────────────────────────

// Design module
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<IRoundRepository, RoundRepository>();
builder.Services.AddScoped<IPouleRepository, PouleRepository>();

// Competition module
builder.Services.AddScoped<ICompetitionRepository, CompetitionRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();

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
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
