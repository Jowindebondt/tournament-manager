using TournamentManager.Domain;

namespace TournamentManager.Application;

public abstract class TableTennisBaseGenerator : ITableTennisGenerator
{
    private ITableTennisGenerator? _next;

    public virtual void Generate(Tournament tournament)
    {
        if (CanHandle(tournament))
        {
            Handle(tournament);
        }
        
        _next?.Generate(tournament);
    }

    public ITableTennisGenerator SetNext(ITableTennisGenerator nextGenerator)
    {
        _next = nextGenerator;
        return this;
    }

    protected abstract void Handle(Tournament tournament);
    protected abstract bool CanHandle(Tournament tournament);
}
