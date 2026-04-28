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
            .ForMember(dest => dest.Sport, opt => opt.MapFrom(src => ParseSport(src.Sport)));

        CreateMap<CreateRoundViewModel, CreateRoundDto>();
        CreateMap<CreatePouleViewModel, CreatePouleDto>();

        CreateMap<RenameTournamentViewModel, RenameTournamentDto>();
        CreateMap<RenameRoundViewModel, RenameRoundDto>();
        CreateMap<RenamePouleViewModel, RenamePouleDto>();

        CreateMap<SetTotalPlayersPouleViewModel, SetTotalPlayersPouleDto>();
        CreateMap<SetPreviousRoundViewModel, SetPreviousRoundDto>();
        CreateMap<SetRoundPoulePositionViewModel, SetRoundPoulePositionDto>();
    }

    private static Sport ParseSport(string value)
    {
        if (!Enum.TryParse<Sport>(value, ignoreCase: true, out var sport))
        {
            throw new ArgumentException($"Invalid sport value: '{value}'.");
        }
        return sport;
    }
}
