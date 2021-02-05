using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class AdditionalWorkProfile : AutoMapper.Profile
    {
        public AdditionalWorkProfile()
        {
            CreateMap<AdditionalWorkCreateRequest, AdditionalWork>();
        }
    }
}
