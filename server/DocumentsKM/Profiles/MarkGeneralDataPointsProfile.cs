using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class MarkGeneralDataPointsProfile : Profile
    {
        public MarkGeneralDataPointsProfile()
        {
            CreateMap<MarkGeneralDataPoint, MarkGeneralDataPointResponse>();
            CreateMap<MarkGeneralDataPointCreateRequest, MarkGeneralDataPoint>();
        }
    }
}
