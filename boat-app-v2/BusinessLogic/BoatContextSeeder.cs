using boat_app_v2.Entities.Models;

namespace boat_app_v2.BusinessLogic;

public static class BoatContextSeeder
{
    public static void SeedData(this BoatContext context)
    {
        if (!context.Boats!.Any())
        {
            context.Boats!.AddRange(
                new Boat { Code = "ABCD-1234-C1", Name = "Sir Jon's Cabin Cruiser", Length = 13.716,  Width = 2.5908},
                new Boat { Code = "ABCD-1234-B1", Name = "Sir cAN's Cabin Cruiser",  Length = 13.716, Width = 2.5908},
                new Boat { Code = "ABCD-1234-B2", Name = "Sir jAMIE's Cabin Cruiser", Length = 13.716, Width = 2.5908},
                new Boat { Code = "AACD-1234-B2", Name = "Sir's Cabin Cruiser", Length = 13.716, Width = 2.5908},
                new Boat { Code = "AACD-1134-C2", Name = "Sir jAMIE's Cabin Cruiser", Length = 13.716,  Width = 2.5908 });

            context.SaveChanges();
        }
    }
}