using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class DocsProfile : Profile
    {
        public DocsProfile()
        {
            CreateMap<Doc, SheetResponse>();
            CreateMap<Doc, DocResponse>();
            CreateMap<DocCreateRequest, Doc>();
        }
    }
}
