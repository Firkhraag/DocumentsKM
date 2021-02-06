using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class NodesProfile : AutoMapper.Profile
    {
        public NodesProfile()
        {
            CreateMap<Node, NodeResponse>();
        }
    }
}
