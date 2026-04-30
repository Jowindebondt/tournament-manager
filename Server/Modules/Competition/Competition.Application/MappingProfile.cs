using AutoMapper;
using Competition.Application.DTOs;
using Competition.Domain.Entities;

namespace Competition.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Match, MatchDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.PouleId, opt => opt.MapFrom(src => src.PouleId.Value))
            .ForMember(dest => dest.Player1Score, opt => opt.MapFrom(src => src.Result != null ? (short?)src.Result.Player1Score : null))
            .ForMember(dest => dest.Player2Score, opt => opt.MapFrom(src => src.Result != null ? (short?)src.Result.Player2Score : null));
    }
}
