using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class GeneralDataSectionsProfile : AutoMapper.Profile
    {
        public GeneralDataSectionsProfile()
        {
            CreateMap<GeneralDataSection, GeneralDataSectionResponse>();
        }
    }
}
