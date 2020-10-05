using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class MarksProfile : Profile
    {
        public MarksProfile()
        {
            // Souce -> Target
            CreateMap<Mark, MarkCodeResponse>();
            CreateMap<Mark, MarkResponse>();
            CreateMap<MarkCreateRequest, Mark>();
        }
    }
}
