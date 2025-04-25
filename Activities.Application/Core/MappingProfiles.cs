using Activities.Domain;
using AutoMapper;

namespace Activities.Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Activity, Activity>();
        CreateMap<Activity, ActivityDTO>();
    }
}