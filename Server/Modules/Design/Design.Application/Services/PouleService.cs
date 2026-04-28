using AutoMapper;
using Design.Application.DTOs;
using Design.Application.Interfaces;
using Design.Domain.Entities;
using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;

namespace Design.Application.Services;

public class PouleService : IPouleService
{
    private readonly IMapper _mapper;
    private readonly IPouleRepository _pouleRepository;
    private readonly IRoundService _roundService;

    public PouleService(IMapper mapper, IPouleRepository pouleRepository, IRoundService roundService)
    {
        _mapper = mapper;
        _pouleRepository = pouleRepository;
        _roundService = roundService;
    }

    public async Task<IEnumerable<PouleDto>> GetAllByRoundAndTournamentAsync(Guid roundId, Guid tournamentId)
    {
        var poules = await _pouleRepository.GetAllByRoundAndTournamentAsync(new TournamentId(tournamentId), new RoundId(roundId));
        return _mapper.Map<IEnumerable<PouleDto>>(poules);
    }

    public async Task<PouleDto> GetByIdAsync(Guid id)
    {
        var poule = await _pouleRepository.GetByIdAsync(new PouleId(id));
        return _mapper.Map<PouleDto>(poule);
    }

    public async Task<PouleDto> CreateAsync(CreatePouleDto createPoule)
    {
        var round = await _roundService.GetByIdAsync(createPoule.RoundId);

        var pouleId = new PouleId(Guid.NewGuid());
        var pouleName = PouleName.Create(createPoule.Name);
        var pouleTotalPlayers = PoulePlayersCount.Create((short)createPoule.TotalPlayers);
        var poule = new Poule(pouleId, pouleName, pouleTotalPlayers, new RoundId(round.Id));

        await _pouleRepository.AddAsync(poule);

        return _mapper.Map<PouleDto>(poule);
    }

    public async Task RenameAsync(RenamePouleDto renamePoule)
    {
        var poule = await _pouleRepository.GetByIdAsync(new PouleId(renamePoule.Id)) ?? throw new ArgumentException("Poule not found");
        poule.Rename(PouleName.Create(renamePoule.Name));
        await _pouleRepository.UpdateAsync(poule);
    }

    public async Task SetTotalPlayersAsync(SetTotalPlayersPouleDto setTotalPlayersPoule)
    {
        var poule = await _pouleRepository.GetByIdAsync(new PouleId(setTotalPlayersPoule.Id)) ?? throw new ArgumentException("Poule not found");
        poule.SetTotalPlayers(PoulePlayersCount.Create((short)setTotalPlayersPoule.TotalPlayers));
        await _pouleRepository.UpdateAsync(poule);
    }

    public async Task DeleteAsync(Guid id)
    {
        var poule = await _pouleRepository.GetByIdAsync(new PouleId(id)) ?? throw new ArgumentException("Poule not found");
        await _pouleRepository.RemoveAsync(poule);
    }
}
