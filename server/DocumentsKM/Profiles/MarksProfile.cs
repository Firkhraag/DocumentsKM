using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Model;

namespace DocumentsKM.Profiles
{
    public class MarksProfile : Profile
    {
        public MarksProfile()
        {
            // Souce -> Target
            CreateMap<Mark, MarkCodeReadDto>();
            CreateMap<Mark, MarkReadDto>();
            CreateMap<MarkCreateDto, Mark>();
            CreateMap<MarkUpdateDto, Mark>();
            CreateMap<Mark, MarkUpdateDto>();
        }
    }
}
