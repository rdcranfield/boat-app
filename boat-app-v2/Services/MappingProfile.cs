using AutoMapper;
using boat_app_v2.Entities.DataTransferObjects;
using boat_app_v2.Entities.Models;

namespace boat_app_v2.Services;

public class MappingProfile : Profile 
{
    public MappingProfile()
    {
        CreateMap<Boat, BoatObject>();
        CreateMap<BoatObject, Boat>();
    }
}