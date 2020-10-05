using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class SubnodesProfile : Profile
    {
        public SubnodesProfile()
        {
            // Souce -> Target
            CreateMap<Subnode, SubnodeCodeResponse>();
            CreateMap<Subnode, SubnodeResponse>();
            // CreateMap<Subnode, SubnodeWithNodeReadDto>();
        }
    }
}
