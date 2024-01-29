using Microsoft.EntityFrameworkCore;

namespace boat_app_v2.BusinessLogic.Repository;

public class RepositoryController : IRepositoryController
{
    private BoatContext _context; 
    private IBoatRepository _boatRepo; 
    
    public RepositoryController(BoatContext context) 
    { 
        _context = context; 
    } 
    public IBoatRepository BoatRepository 
    { 
        get 
        { 
            if (_boatRepo == null) 
            { 
                _boatRepo = new BoatRepository(_context); 
            } 
            return _boatRepo; 
        } 
    } 
    
    public void Save() 
    {
        _context.SaveChanges();
    } 
}