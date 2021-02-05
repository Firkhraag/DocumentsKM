using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class ConstructionSubtypesProfile : AutoMapper.Profile
    {
        public ConstructionSubtypesProfile()
        {
            CreateMap<ConstructionSubtype, ConstructionSubtypeResponse>();
        }
    }
}
