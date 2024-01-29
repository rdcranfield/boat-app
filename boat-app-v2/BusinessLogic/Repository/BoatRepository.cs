using boat_app_v2.Entities.Models;
using boat_app_v2.Models;
using Microsoft.EntityFrameworkCore;

namespace boat_app_v2.BusinessLogic.Repository;

public class BoatRepository : Repository<Boat>, IBoatRepository 
{
    public BoatRepository(BoatContext context) : base(context)
    {
    }

    public IEnumerable<Boat> GetAllBoats()
    {
        return FindAll()
            .OrderBy(boat => boat.Code)
            .ToList();    
    }

    public Boat GetBoatById(string id)
    {
        return FindByCondition(boat => boat.Code.Equals(id)).FirstOrDefault();
    }

    public void CreateBoat(Boat boat) => Create(boat);

    public void UpdateBoat(Boat boat) => Update(boat);

    public void DeleteBoat(Boat boat) => Delete(boat);
}