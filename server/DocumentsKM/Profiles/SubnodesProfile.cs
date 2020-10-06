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
            CreateMap<Subnode, SubnodeBaseResponse>();
            CreateMap<Subnode, SubnodeParentResponse>();
            CreateMap<Subnode, SubnodeResponse>();
        }
    }
}
