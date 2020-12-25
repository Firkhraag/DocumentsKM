using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class AdditionalWorkProfile : Profile
    {
        public AdditionalWorkProfile()
        {
            CreateMap<AdditionalWork, AdditionalWorkResponse>();
            CreateMap<AdditionalWorkCreateRequest, AdditionalWork>();
            CreateMap<AdditionalWorkUpdateRequest, AdditionalWork>();
        }
    }
}
