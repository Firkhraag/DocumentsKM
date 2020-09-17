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
            CreateMap<Mark, MarkWithSubnodeReadDto>();

            CreateMap<Mark, MarkReadDto>();
            CreateMap<MarkCreateDto, Mark>();
            CreateMap<MarkUpdateDto, Mark>();
            CreateMap<Mark, MarkUpdateDto>();

            // CreateMap<Book, BookDTO>()
            //     .ForMember(m => m.Id, opt => opt.Ignore());

            // CreateMap<Author, AuthorDTO>()
            //     .ForMember(m => m.Id, opt => opt.Ignore());
        }
    }
}
