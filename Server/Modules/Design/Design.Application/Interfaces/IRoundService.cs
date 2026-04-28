using Design.Application.DTOs;
using Design.Domain.ValueObjects;

namespace Design.Application.Interfaces;

public interface IRoundService
{
    Task<IEnumerable<RoundDto>> GetAllByTournamentAsync(Guid tournamentId);
    Task<RoundDto> GetByIdAsync(Guid id);
    Task<RoundDto> CreateAsync(CreateRoundDto createRound);
    Task RenameAsync(RenameRoundDto renameRound);
    Task SetPreviousRoundAsync(SetPreviousRoundDto setPreviousRound);
    Task SetRoundSettingsAsync(Guid id, RoundSettings settings);
    Task SetRoundPoulePositionAsync(SetRoundPoulePositionDto setRoundPoulePosition);
    Task DeleteAsync(Guid id);
}
