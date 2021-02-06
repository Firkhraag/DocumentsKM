using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class SubnodesProfile : AutoMapper.Profile
    {
        public SubnodesProfile()
        {
            CreateMap<Subnode, SubnodeResponse>();
        }
    }
}
