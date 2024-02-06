using boat_app_v2.Entities.Models;

namespace boat_app_v2.BusinessLogic.Repository;

public interface IBoatRepository
{
    Task<IEnumerable<Boat?>> GetAllBoatsAsync();
    Task<Boat?> GetBoatByIdAsync(string id);
    void CreateBoat(Boat boat);
    void UpdateBoat(Boat boat);
    void DeleteBoat(Boat boat);
}