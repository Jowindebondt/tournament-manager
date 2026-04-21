using AutoMapper;
using Design.Application.DTOs;
using Design.Domain;
using Design.Domain.Entities;
using Design.Domain.ValueObjects;

namespace Design.Application.Services;

public class RoundService : IRoundService
{
    private readonly IMapper _mapper;
    private readonly IRoundRepository _roundRepository;
    private readonly ITournamentService _tournamentService;
    private readonly IPouleService _pouleService;

    public RoundService(IMapper mapper, IRoundRepository roundRepository, ITournamentService tournamentService, IPouleService pouleService)
    {
        _mapper = mapper;
        _roundRepository = roundRepository;
        _tournamentService = tournamentService;
        _pouleService = pouleService;
    }

    public async Task<IEnumerable<RoundDto>> GetAllByTournamentAsync(Guid tournamentId)
    {
        var rounds = await _roundRepository.GetAllByTournamentAsync(new TournamentId(tournamentId));
        return _mapper.Map<IEnumerable<RoundDto>>(rounds);
    }

    public async Task<RoundDto> GetByIdAsync(Guid id)
    {
        var round = await _roundRepository.GetByIdAsync(new RoundId(id));
        return _mapper.Map<RoundDto>(round);
    }

    public async Task<RoundDto> CreateAsync(CreateRoundDto createRound)
    {
        var tournament = await _tournamentService.GetByIdAsync(createRound.TournamentId);

        var roundId = new RoundId(Guid.NewGuid());
        var roundName = RoundName.Create(createRound.Name);
        var round = new Round(roundId, roundName, new TournamentId(tournament.Id));
        
        await _roundRepository.AddAsync(round);

        return _mapper.Map<RoundDto>(round);
    }

    public async Task RenameAsync(RenameRoundDto renameRound)
    {
        var round = await _roundRepository.GetByIdAsync(new RoundId(renameRound.Id)) ?? throw new ArgumentException("Round not found");
        
        round.Rename(RoundName.Create(renameRound.Name));

        await _roundRepository.UpdateAsync(round);
    }

    public async Task SetPreviousRoundAsync(SetPreviousRoundDto setPreviousRound)
    {
        var round = await _roundRepository.GetByIdAsync(new RoundId(setPreviousRound.Id)) ?? throw new ArgumentException("Round not found");
        var previousRound = await _roundRepository.GetByIdAsync(new RoundId(setPreviousRound.PreviousId)) ?? throw new ArgumentException("Previous round not found");

        round.SetPreviousRound(previousRound);
        
        await _roundRepository.UpdateAsync(round);
    }

    public async Task SetTableTennisSettingsAsync(SetTableTennisRoundSettingsDto setTableTennisRoundSettings)
    {
        var round = await _roundRepository.GetByIdAsync(new RoundId(setTableTennisRoundSettings.Id)) ?? throw new ArgumentException("Round not found");

        round.SetSettings(TableTennisRoundSettings.Create(setTableTennisRoundSettings.BestOf));
        
        await _roundRepository.UpdateAsync(round);
    }

    public async Task SetRoundPoulePositionAsync(SetRoundPoulePositionDto setRoundPoulePosition)
    {
        var round = await _roundRepository.GetByIdAsync(new RoundId(setRoundPoulePosition.Id)) ?? throw new ArgumentException("Round not found");

        if (round.PreviousRound == null)
        {
            throw new ArgumentException("Round has no previous round configured");
        }

        var currentPoule = await _pouleService.GetByIdAsync(setRoundPoulePosition.CurrentPouleId);
        if (currentPoule.Round.Id != round.Id.Value)
        {
            throw new ArgumentException("Current poule is not part of round");
        }

        var previousPoule = await _pouleService.GetByIdAsync(setRoundPoulePosition.PreviousPouleId);
        if (previousPoule.Round.Id != round.PreviousRound.Id.Value)
        {
            throw new ArgumentException("Previous poule is not part of previous round");
        }

        var currentPoulePosition = PoulePosition.Create(_mapper.Map<Poule>(currentPoule), setRoundPoulePosition.CurrentPosition);
        var previousPoulePosition = PoulePosition.Create(_mapper.Map<Poule>(previousPoule), setRoundPoulePosition.PreviousPosition);
        var mapping = PoulePositionMapping.Create(previousPoulePosition, currentPoulePosition, round.Settings);
        round.Settings.PoulePositions.Add(mapping);

        await _roundRepository.UpdateAsync(round);
    }

    public async Task DeleteAsync(Guid id)
    {
        var round = await _roundRepository.GetByIdAsync(new RoundId(id)) ?? throw new ArgumentException("Round not found");
        await _roundRepository.RemoveAsync(round);
    }
}
