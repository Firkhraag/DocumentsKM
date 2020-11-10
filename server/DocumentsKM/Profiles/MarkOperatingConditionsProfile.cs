using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class MarkOperatingConditionsProfile : Profile
    {
        public MarkOperatingConditionsProfile()
        {
            CreateMap<MarkOperatingConditions, MarkOperatingConditionsResponse>();
            CreateMap<MarkOperatingConditionsCreateRequest, MarkOperatingConditions>();
        }
    }
}
