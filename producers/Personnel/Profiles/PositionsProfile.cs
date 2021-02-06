using AutoMapper;
using Personnel.Dtos;
using Personnel.Models;

namespace Personnel.Profiles
{
    public class PositionsProfile : Profile
    {
        public PositionsProfile()
        {
            CreateMap<PositionRequest, Position>();
        }
    }
}
