using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class AdditionalWorkProfile : Profile
    {
        public AdditionalWorkProfile()
        {
            CreateMap<AdditionalWorkCreateRequest, AdditionalWork>();
        }
    }
}
