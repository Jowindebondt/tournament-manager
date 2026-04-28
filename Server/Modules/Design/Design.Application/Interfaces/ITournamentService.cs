using Design.Application.DTOs;

namespace Design.Application.Interfaces;

public interface ITournamentService
{
    Task<IEnumerable<TournamentDto>> GetAllAsync();
    Task<TournamentDto> GetByIdAsync(Guid id);
    Task<TournamentDto> CreateAsync(CreateTournamentDto createTournament);
    Task RenameAsync(RenameTournamentDto renameTournament);
    Task DeleteAsync(Guid id);
    Task GenerateAsync(Guid id);
}
