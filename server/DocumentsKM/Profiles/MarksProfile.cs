using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class MarksProfile : AutoMapper.Profile
    {
        public MarksProfile()
        {
            CreateMap<Mark, MarkResponse>();
            CreateMap<MarkCreateRequest, Mark>();
        }
    }
}
