using boat_app_v2.BusinessLogic.Repository;
using boat_app_v2.Entities.Models;
using boat_app_v2.Models;

namespace boat_app_v2.Services;

public class BoatService
{
    private readonly IRepository<Boat> _boatRepositoryWrapper;
    public IRepositoryController repository;

    public BoatService(IRepositoryController repository)
    {
        this.repository = repository;
        
    }
}