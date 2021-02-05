using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class ConstructionsProfile : AutoMapper.Profile
    {
        public ConstructionsProfile()
        {
            CreateMap<Construction, ConstructionResponse>();
            CreateMap<ConstructionCreateRequest, Construction>();
        }
    }
}
