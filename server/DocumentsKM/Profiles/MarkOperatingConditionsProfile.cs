using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class MarkOperatingConditionsProfile : AutoMapper.Profile
    {
        public MarkOperatingConditionsProfile()
        {
            CreateMap<MarkOperatingConditions, MarkOperatingConditionsResponse>();
            CreateMap<MarkOperatingConditionsCreateRequest, MarkOperatingConditions>();
        }
    }
}
