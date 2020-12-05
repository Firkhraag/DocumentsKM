using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class NodesProfile : Profile
    {
        public NodesProfile()
        {
            CreateMap<Node, NodeResponse>();
        }
    }
}
