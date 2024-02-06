using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace boat_app_v2.BusinessLogic.Repository;

public abstract class Repository<T> : IRepository<T> where T : class
{
    private readonly BoatContext _context;

    protected Repository(BoatContext context)
    {
        _context = context;
    }

    public IQueryable<T?>  FindAll() => _context.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) => 
        _context.Set<T>().Where(expression).AsNoTracking();

    public void Create(T entity) => _context.Set<T>().Add(entity);

    public void Update(T entity) => _context.Set<T>().Update(entity);

    public void Delete(T entity)
    {
       _context.Set<T>().Remove(entity);
    } 
}