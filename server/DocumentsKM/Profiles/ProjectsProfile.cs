using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Model;

namespace DocumentsKM.Profiles
{
    public class ProjectsProfile : Profile
    {
        public ProjectsProfile()
        {
            // Souce -> Target
            CreateMap<Project, ProjectSeriesReadDto>();
        }
    }
}
