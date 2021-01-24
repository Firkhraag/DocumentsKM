using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class StandardConstructionsProfile : Profile
    {
        public StandardConstructionsProfile()
        {
            CreateMap<StandardConstruction, StandardConstructionResponse>();
            CreateMap<StandardConstructionCreateRequest, StandardConstruction>();
        }
    }
}
