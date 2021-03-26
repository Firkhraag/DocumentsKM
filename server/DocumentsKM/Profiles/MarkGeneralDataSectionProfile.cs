using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class MarkGeneralDataSectionsProfile : AutoMapper.Profile
    {
        public MarkGeneralDataSectionsProfile()
        {
            CreateMap<MarkGeneralDataSection, MarkGeneralDataSectionResponse>();
            CreateMap<MarkGeneralDataSectionCreateRequest, MarkGeneralDataSection>();
        }
    }
}
