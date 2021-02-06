using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class MarkGeneralDataPointsProfile : AutoMapper.Profile
    {
        public MarkGeneralDataPointsProfile()
        {
            CreateMap<MarkGeneralDataPoint, MarkGeneralDataPointResponse>();
            CreateMap<MarkGeneralDataPointCreateRequest, MarkGeneralDataPoint>();
        }
    }
}
