using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class MarkLinkedDocsProfile : AutoMapper.Profile
    {
        public MarkLinkedDocsProfile()
        {
            CreateMap<MarkLinkedDoc, MarkLinkedDocResponse>();
            CreateMap<MarkLinkedDocCreateRequest, MarkLinkedDoc>();
        }
    }
}
