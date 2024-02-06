using boat_app_v2.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace boat_app_v2.BusinessLogic.Repository;

public class BoatRepository : Repository<Boat>, IBoatRepository 
{
    public BoatRepository(BoatContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Boat?>> GetAllBoatsAsync()
    {
        return await FindAll()
            .OrderBy(boat => boat!.Code).ToListAsync();
    }

    public Task<Boat?> GetBoatByIdAsync(string id)
    {
        return FindByCondition(boat => boat.Code!.Equals(id)).FirstOrDefaultAsync();
    }

    public void CreateBoat(Boat boat) => Create(boat);

    public void UpdateBoat(Boat boat) => Update(boat);

    public void DeleteBoat(Boat boat) => Delete(boat);
}