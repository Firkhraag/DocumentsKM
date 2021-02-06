using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class ConstructionBoltsProfile : AutoMapper.Profile
    {
        public ConstructionBoltsProfile()
        {
            CreateMap<ConstructionBolt, ConstructionBoltResponse>();
            CreateMap<ConstructionBoltCreateRequest, ConstructionBolt>();
        }
    }
}
