using Design.Application.DTOs;

namespace Design.Application.Services;

public interface ITournamentService
{
    Task<IEnumerable<TournamentDTO>> GetAllAsync();
    Task<TournamentDTO> GetByIdAsync(Guid id);
    Task<TournamentDTO> CreateAsync(CreateTournamentDTO createTournament);
    Task RenameAsync(RenameTournamentDTO renameTournament);
    Task LoadTemplateAsync(Guid id, Guid templateId);
    Task DeleteAsync(Guid id);
    Task GenerateAsync(Guid id);
}
