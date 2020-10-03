using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class ProjectsProfile : Profile
    {
        public ProjectsProfile()
        {
            // Souce -> Target
            CreateMap<Project, ProjectSeriesResponse>();
            // CreateMap<Project, ProjectWithNodesReadDto>();
        }
    }
}
