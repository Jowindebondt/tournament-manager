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
            .ForMember(dest => dest.CompetitionType, opt => opt.MapFrom((src, _) => GetCompetitionType(src.Type)))
            .ForMember(dest => dest.KnockOutPhase, opt => opt.MapFrom((src, _) => GetKnockOutPhase(src.Type)));

        CreateMap<Poule, PouleDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ForMember(dest => dest.TotalPlayers, opt => opt.MapFrom(src => src.TotalPlayers.Value));
    }

    private static string? GetCompetitionType(RoundType? type) => type switch
    {
        null => null,
        RoundRobinType => "RoundRobin",
        KnockOutType => "KnockOut",
        _ => null
    };

    private static string? GetKnockOutPhase(RoundType? type) =>
        type is KnockOutType ko ? ko.Phase.ToString() : null;
}
