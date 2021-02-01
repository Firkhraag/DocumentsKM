using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class ConstructionElementsProfile : Profile
    {
        public ConstructionElementsProfile()
        {
            CreateMap<ConstructionElement, ConstructionElementResponse>();
            CreateMap<ConstructionElementCreateRequest, ConstructionElement>();
        }
    }
}
