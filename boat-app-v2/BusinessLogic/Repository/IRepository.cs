using System.Linq.Expressions;

namespace boat_app_v2.BusinessLogic.Repository;

public interface IRepository<T>  
{
    IEnumerable<T?> FindAll();
    IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression); 
    void Create(T entity); 
    void Update(T entity); 
    void Delete(T entity);
}