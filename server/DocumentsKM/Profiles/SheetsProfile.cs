using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class SheetsProfile : Profile
    {
        public SheetsProfile()
        {
            CreateMap<Sheet, SheetResponse>();
        }
    }
}
