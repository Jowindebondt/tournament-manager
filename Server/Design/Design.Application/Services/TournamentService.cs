using AutoMapper;
using Design.Application.DTOs;
using Design.Domain;
using Design.Domain.Entities;
using Design.Domain.ValueObjects;

namespace Design.Application.Services;

public class TournamentService : ITournamentService
{
    private readonly IMapper _mapper;
    private readonly ITournamentRepository _tournamentRepository;

    public TournamentService(IMapper mapper, ITournamentRepository tournamentRepository)
    {
        _mapper = mapper;
        _tournamentRepository = tournamentRepository;
    }

    public async Task<IEnumerable<TournamentDTO>> GetAllAsync()
    {
        var tournaments = await _tournamentRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<TournamentDTO>>(tournaments);
    }

    public async Task<TournamentDTO> GetByIdAsync(Guid id)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(new TournamentId(id));
        return _mapper.Map<TournamentDTO>(tournament);
    }

    public async Task<TournamentDTO> CreateAsync(CreateTournamentDTO createTournament)
    {
        var tournamentId = new TournamentId(Guid.NewGuid());
        var tournamentName = TournamentName.Create(createTournament.Name);
        var tournament = new Tournament(tournamentId, tournamentName, createTournament.Sport);

        await _tournamentRepository.AddAsync(tournament);
        
        return _mapper.Map<TournamentDTO>(tournament);
    }

    public async Task RenameAsync(RenameTournamentDTO renameTournament)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(new TournamentId(renameTournament.Id)) ?? throw new ArgumentException("Tournament not found");
        tournament.Rename(TournamentName.Create(renameTournament.Name));
        await _tournamentRepository.UpdateAsync(tournament);
    }

    public async Task LoadTemplateAsync(Guid id, Guid templateId)
    {

    }

    public async Task DeleteAsync(Guid id)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(new TournamentId(id)) ?? throw new ArgumentException("Tournament not found");
        await _tournamentRepository.RemoveAsync(tournament);
    }

    public async Task GenerateAsync(Guid id)
    {

    }
}
