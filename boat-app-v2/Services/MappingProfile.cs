using AutoMapper;
using boat_app_v2.Entities.DataTransferObjects;
using boat_app_v2.Entities.Models;
using boat_app_v2.Models;

namespace boat_app_v2.Services;

public class MappingProfile : Profile 
{
    public MappingProfile()
    {
        CreateMap<Boat, BoatObject>();
        CreateMap<BoatObject, Boat>();

       // CreateMap<List<Boat>, List<BoatObject>>();

        //CreateMap<BoatCreateObject, Boat>();
       // CreateMap<Boat, BoatCreateObject>();

        // CreateMap<BoatUpdateObject, Boat>();
        // CreateMap<Boat, BoatUpdateObject>();
    }
}