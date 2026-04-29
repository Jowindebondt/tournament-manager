using AutoMapper;
using Design.Application.DTOs;
using Design.Domain.Entities;
using Design.Domain.ValueObjects;

namespace Design.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Tournament, TournamentDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ForMember(dest => dest.Sport, opt => opt.MapFrom(src => src.Sport));

        CreateMap<Round, RoundDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => MapRoundType(src.Type)));

        CreateMap<Poule, PouleDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ForMember(dest => dest.TotalPlayers, opt => opt.MapFrom(src => src.TotalPlayers.Value));
    }

    private static string? MapRoundType(RoundType? roundType) => roundType switch
    {
        RoundRobinType => "RoundRobin",
        KnockOutType knockOut => $"KnockOut:{knockOut.Phase}",
        null => null,
        _ => throw new ArgumentOutOfRangeException(nameof(roundType), "Unsupported round type.")
    };
}
