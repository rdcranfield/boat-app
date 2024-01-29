using System.Linq.Expressions;
using boat_app_v2.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace boat_app_v2.BusinessLogic.Repository;

public abstract class Repository<T> : IRepository<T> where T : class
{
    private readonly BoatContext _context;
   // private readonly DbSet<T> _dbSet;
    public Repository(BoatContext context)
    {
        _context = context;
        //_dbSet = _context.Set<T>();
    }

    public IEnumerable<T?>  FindAll() => _context.Set<T>();

    public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression) => 
        _context.Set<T>().Where(expression).AsNoTracking();

    public void Create(T entity) => _context.Set<T>().Add(entity);

    public void Update(T entity) => _context.Set<T>().Update(entity);

    public void Delete(T entity)
    {
       _context.Set<T>().Remove(entity);
    } 
    
    /*public T? GetById(string id)
    {
        return _dbSet.Find(id);
    }

    public IEnumerable<T?> GetAll()
    {
        return _dbSet.ToList();
    }

    public void Create(T? entity)
    {
        _dbSet.Add(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T? entity)
    {
        _dbSet.Remove(entity);
    }*/
}