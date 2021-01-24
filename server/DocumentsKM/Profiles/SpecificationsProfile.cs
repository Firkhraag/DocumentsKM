using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class SpecificationsProfile : Profile
    {
        public SpecificationsProfile()
        {
            CreateMap<Specification, SpecificationResponse>();
        }
    }
}
