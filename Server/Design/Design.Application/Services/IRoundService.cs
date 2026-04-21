using Design.Application.DTOs;

namespace Design.Application.Services;

public interface IRoundService
{
    Task<IEnumerable<RoundDto>> GetAllByTournamentAsync(Guid tournamentId);
    Task<RoundDto> GetByIdAsync(Guid id);
    Task<RoundDto> CreateAsync(CreateRoundDto createRound);
    Task RenameAsync(RenameRoundDto renameRound);
    Task SetPreviousRoundAsync(SetPreviousRoundDto setPreviousRound);
    Task SetTableTennisSettingsAsync(SetTableTennisRoundSettingsDto setTableTennisRoundSettings);
    Task SetRoundPoulePositionAsync(SetRoundPoulePositionDto setRoundPoulePosition);
    Task DeleteAsync(Guid id);
}
