using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class ConstructionBoltsProfile : Profile
    {
        public ConstructionBoltsProfile()
        {
            CreateMap<ConstructionBolt, ConstructionBoltResponse>();
            CreateMap<ConstructionBoltCreateRequest, ConstructionBolt>();
            CreateMap<ConstructionBoltUpdateRequest, ConstructionBolt>();
        }
    }
}
