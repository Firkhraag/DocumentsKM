using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class EstimateTaskProfile : AutoMapper.Profile
    {
        public EstimateTaskProfile()
        {
            CreateMap<EstimateTask, EstimateTaskResponse>();
            CreateMap<EstimateTaskCreateRequest, EstimateTask>();
        }
    }
}
