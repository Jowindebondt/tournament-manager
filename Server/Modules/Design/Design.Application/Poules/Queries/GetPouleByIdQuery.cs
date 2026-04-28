using AutoMapper;
using Design.Application.DTOs;
using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;

namespace Design.Application.Poules.Queries;

public record GetPouleByIdQuery(Guid Id) : IRequest<PouleDto?>;

public class GetPouleByIdQueryHandler(IMapper mapper, IPouleRepository pouleRepository)
    : IRequestHandler<GetPouleByIdQuery, PouleDto?>
{
    public async Task<PouleDto?> Handle(GetPouleByIdQuery request, CancellationToken cancellationToken)
    {
        var poule = await pouleRepository.GetByIdAsync(new PouleId(request.Id));
        return mapper.Map<PouleDto?>(poule);
    }
}
