using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class MarkLinkedDocProfile : Profile
    {
        public MarkLinkedDocProfile()
        {
            CreateMap<MarkLinkedDoc, MarkLinkedDocResponse>();
            CreateMap<MarkLinkedDocCreateRequest, MarkLinkedDoc>();
            CreateMap<MarkLinkedDocUpdateRequest, MarkLinkedDoc>();
        }
    }
}
