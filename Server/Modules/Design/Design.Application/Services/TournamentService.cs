using AutoMapper;
using Design.Application.DTOs;
using Design.Application.Interfaces;
using Design.Domain.Entities;
using Design.Domain.Interfaces;
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

    public async Task<IEnumerable<TournamentDto>> GetAllAsync()
    {
        var tournaments = await _tournamentRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
    }

    public async Task<TournamentDto> GetByIdAsync(Guid id)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(new TournamentId(id));
        return _mapper.Map<TournamentDto>(tournament);
    }

    public async Task<TournamentDto> CreateAsync(CreateTournamentDto createTournament)
    {
        var tournamentId = new TournamentId(Guid.NewGuid());
        var tournamentName = TournamentName.Create(createTournament.Name);
        var tournament = new Tournament(tournamentId, tournamentName, createTournament.Sport);

        await _tournamentRepository.AddAsync(tournament);

        return _mapper.Map<TournamentDto>(tournament);
    }

    public async Task RenameAsync(RenameTournamentDto renameTournament)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(new TournamentId(renameTournament.Id)) ?? throw new ArgumentException("Tournament not found");
        tournament.Rename(TournamentName.Create(renameTournament.Name));
        await _tournamentRepository.UpdateAsync(tournament);
    }

    public async Task DeleteAsync(Guid id)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(new TournamentId(id)) ?? throw new ArgumentException("Tournament not found");
        await _tournamentRepository.RemoveAsync(tournament);
    }

    public Task GenerateAsync(Guid id)
    {
        // TODO: Implement tournament generation logic
        return Task.CompletedTask;
    }
}
