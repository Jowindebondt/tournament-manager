using AutoMapper;
using Design.Api.ViewModels;
using Design.Application.DTOs;
using Design.Domain.Enums;

namespace Design.Api;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TournamentDto, TournamentViewModel>()
            .ForMember(dest => dest.Sport, opt => opt.MapFrom(src => src.Sport.ToString()));

        CreateMap<RoundDto, RoundViewModel>();
        CreateMap<PouleDto, PouleViewModel>();

        CreateMap<CreateTournamentViewModel, CreateTournamentDto>()
            .ForMember(dest => dest.Sport, opt => opt.MapFrom(src => Enum.Parse<Sport>(src.Sport, true)));

        CreateMap<CreateRoundViewModel, CreateRoundDto>();
        CreateMap<CreatePouleViewModel, CreatePouleDto>();

        CreateMap<RenameTournamentViewModel, RenameTournamentDto>();
        CreateMap<RenameRoundViewModel, RenameRoundDto>();
        CreateMap<RenamePouleViewModel, RenamePouleDto>();

        CreateMap<SetTotalPlayersPouleViewModel, SetTotalPlayersPouleDto>();
        CreateMap<SetPreviousRoundViewModel, SetPreviousRoundDto>();
        CreateMap<SetRoundPoulePositionViewModel, SetRoundPoulePositionDto>();
    }
}
