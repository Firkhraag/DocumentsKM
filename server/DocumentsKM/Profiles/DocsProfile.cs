using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class DocsProfile : AutoMapper.Profile
    {
        public DocsProfile()
        {
            CreateMap<Doc, SheetResponse>();
            CreateMap<Doc, DocResponse>();
            CreateMap<DocCreateRequest, Doc>();
        }
    }
}
