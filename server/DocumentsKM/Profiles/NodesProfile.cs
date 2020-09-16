using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Model;

namespace DocumentsKM.Profiles
{
    public class NodesProfile : Profile
    {
        public NodesProfile()
        {
            // Souce -> Target
            CreateMap<Node, NodeCodeReadDto>();
        }
    }
}
