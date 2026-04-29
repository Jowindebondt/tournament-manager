using System.Diagnostics;
using CSharpFunctionalExtensions;
using Competition.Domain.ValueObjects;

namespace Competition.Domain.Entities;

[DebuggerDisplay($"{nameof(Name)} = {{{nameof(Name)}}}")]
public sealed class Competitor : Entity<CompetitorId>
{
    public CompetitorName Name { get; private set; }

    public CompetitionPouleId CompetitionPouleId { get; private set; }
    public CompetitionPoule CompetitionPoule { get; private set; } = null!;

    public Competitor(CompetitorId id, CompetitorName name, CompetitionPouleId competitionPouleId) : base(id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentNullException.ThrowIfNull(competitionPouleId, nameof(competitionPouleId));

        Name = name;
        CompetitionPouleId = competitionPouleId;
    }

    public void Rename(CompetitorName newName)
    {
        ArgumentNullException.ThrowIfNull(newName, nameof(newName));

        Name = newName;
    }
}
