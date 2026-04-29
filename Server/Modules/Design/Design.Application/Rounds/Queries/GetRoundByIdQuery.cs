using AutoMapper;
using Design.Application.DTOs;
using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;

namespace Design.Application.Rounds.Queries;

public record GetRoundByIdQuery(Guid Id) : IRequest<RoundDto?>;

public class GetRoundByIdQueryHandler(IMapper mapper, IRoundRepository roundRepository)
    : IRequestHandler<GetRoundByIdQuery, RoundDto?>
{
    public async Task<RoundDto?> Handle(GetRoundByIdQuery request, CancellationToken cancellationToken)
    {
        var round = await roundRepository.GetByIdAsync(new RoundId(request.Id));
        return mapper.Map<RoundDto?>(round);
    }
}
