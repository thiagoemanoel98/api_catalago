using System.Linq.Expressions;

namespace ApiCatalago.Repositories;

public interface IRepository<T>
{
    // Cuidado pra não violar o principio SOLID ISP
    IEnumerable<T> GetAll();
    
    T? Get(Expression<Func<T, bool>> predicate);

    T Create(T entity);

    T Update(T entity);

    T Delete(T entity);
}