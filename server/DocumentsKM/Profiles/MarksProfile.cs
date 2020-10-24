using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class MarksProfile : Profile
    {
        public MarksProfile()
        {
            CreateMap<Mark, MarkBaseResponse>();
            CreateMap<Mark, MarkParentResponse>();
            CreateMap<Mark, MarkResponse>();
            CreateMap<MarkCreateRequest, Mark>();
        }
    }
}
