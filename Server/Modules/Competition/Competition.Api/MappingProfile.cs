using AutoMapper;
using Competition.Api.ViewModels;
using Competition.Application.DTOs;

namespace Competition.Api;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<MatchDto, MatchViewModel>();
    }
}
