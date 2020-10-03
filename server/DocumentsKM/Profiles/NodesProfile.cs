using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class NodesProfile : Profile
    {
        public NodesProfile()
        {
            // Souce -> Target
            CreateMap<Node, NodeCodeResponse>();
            // CreateMap<Node, NodeWithProjectReadDto>();
        }
    }
}
