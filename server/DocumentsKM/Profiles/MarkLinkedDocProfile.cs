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
            CreateMap<MarkLinkedDocRequest, MarkLinkedDoc>();
        }
    }
}
