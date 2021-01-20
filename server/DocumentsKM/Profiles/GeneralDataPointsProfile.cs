using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class GeneralDataPointsProfile : Profile
    {
        public GeneralDataPointsProfile()
        {
            CreateMap<GeneralDataPoint, GeneralDataPointResponse>();
            CreateMap<GeneralDataPointCreateRequest, GeneralDataPoint>();
            CreateMap<GeneralDataPointUpdateRequest, GeneralDataPoint>();
        }
    }
}
