using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class ConstructionSubtypesProfile : Profile
    {
        public ConstructionSubtypesProfile()
        {
            CreateMap<ConstructionSubtype, ConstructionSubtypeResponse>();
        }
    }
}
