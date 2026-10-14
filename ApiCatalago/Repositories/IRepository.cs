using System.Linq.Expressions;

namespace ApiCatalago.Repositories;

public interface IRepository<T>
{
    // Cuidado pra não violar o principio SOLID ISP
    Task<IEnumerable<T>> GetAllAsync();
    
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);

    T Create(T entity);

    T Update(T entity);

    T Delete(T entity);
}