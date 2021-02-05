using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class StandardConstructionsProfile : AutoMapper.Profile
    {
        public StandardConstructionsProfile()
        {
            CreateMap<StandardConstruction, StandardConstructionResponse>();
            CreateMap<StandardConstructionCreateRequest, StandardConstruction>();
        }
    }
}
