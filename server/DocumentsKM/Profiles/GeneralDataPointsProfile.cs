using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class GeneralDataPointsProfile : AutoMapper.Profile
    {
        public GeneralDataPointsProfile()
        {
            CreateMap<GeneralDataPoint, GeneralDataPointResponse>();
        }
    }
}
