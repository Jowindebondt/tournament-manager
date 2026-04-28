using CSharpFunctionalExtensions;

namespace Design.Domain.ValueObjects;

public abstract class RoundSettings : ValueObject
{
    public IReadOnlyCollection<PoulePositionMapping> PoulePositions { get; } = [];
}
