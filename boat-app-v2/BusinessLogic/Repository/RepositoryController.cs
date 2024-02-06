namespace boat_app_v2.BusinessLogic.Repository;

public class RepositoryController : IRepositoryController
{
    private readonly BoatContext _context; 
    private IBoatRepository? _boatRepo; 

    public RepositoryController(BoatContext context) 
    { 
        _context = context; 
    } 
    public IBoatRepository BoatRepository 
    { 
        get 
        {
            if (_boatRepo != null) return _boatRepo;
            _boatRepo = new BoatRepository(_context);
            return _boatRepo; 
        } 
    } 
    
    public async Task SaveAsync() 
    {
        await _context.SaveChangesAsync();
    } 
}