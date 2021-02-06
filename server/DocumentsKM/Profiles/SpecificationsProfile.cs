using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class SpecificationsProfile : AutoMapper.Profile
    {
        public SpecificationsProfile()
        {
            CreateMap<Specification, SpecificationResponse>();
        }
    }
}
