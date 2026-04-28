using CSharpFunctionalExtensions;

namespace Design.Domain.ValueObjects;

public abstract class RoundSettings : ValueObject
{
    public ICollection<PoulePositionMapping> PoulePositions { get; } = [];
}
