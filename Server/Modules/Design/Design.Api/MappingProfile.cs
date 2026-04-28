using AutoMapper;
using Design.Api.ViewModels;
using Design.Application.DTOs;

namespace Design.Api;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TournamentDto, TournamentViewModel>()
            .ForMember(dest => dest.Sport, opt => opt.MapFrom(src => src.Sport.ToString()));

        CreateMap<RoundDto, RoundViewModel>();
        CreateMap<PouleDto, PouleViewModel>();
    }
}

