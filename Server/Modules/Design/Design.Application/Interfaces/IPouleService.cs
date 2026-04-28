using Design.Application.DTOs;

namespace Design.Application.Interfaces;

public interface IPouleService
{
    Task<IEnumerable<PouleDto>> GetAllByRoundAndTournamentAsync(Guid roundId, Guid tournamentId);
    Task<PouleDto> GetByIdAsync(Guid id);
    Task<PouleDto> CreateAsync(CreatePouleDto createPoule);
    Task RenameAsync(RenamePouleDto renamePoule);
    Task SetTotalPlayersAsync(SetTotalPlayersPouleDto setTotalPlayersPoule);
    Task DeleteAsync(Guid id);
}
