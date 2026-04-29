using CompetitionEntity = Competition.Domain.Entities.Competition;
using Competition.Domain.Entities;
using Competition.Domain.ValueObjects;
using Competition.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Test.Design.Api.Fixtures;

/// <summary>
/// A test-only DbContext that extends CompetitionDbContext with the EF Core model configuration
/// required for the InMemory provider. The production CompetitionDbContext supplies all value
/// converters and relationship mappings; this derived context adds no overrides and exists solely
/// to allow the InMemory provider to be configured via a typed DbContextOptions.
/// </summary>
public class TestCompetitionDbContext : CompetitionDbContext
{
    public TestCompetitionDbContext(DbContextOptions options) : base(options)
    {
    }
}
