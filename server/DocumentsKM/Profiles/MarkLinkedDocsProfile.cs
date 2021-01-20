using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class MarkLinkedDocsProfile : Profile
    {
        public MarkLinkedDocsProfile()
        {
            CreateMap<MarkLinkedDoc, MarkLinkedDocResponse>();
            CreateMap<MarkLinkedDocCreateRequest, MarkLinkedDoc>();
            CreateMap<MarkLinkedDocUpdateRequest, MarkLinkedDoc>();
        }
    }
}
