using System.Linq.Expressions;
using ApiCatalago.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Repositories;

public class Repository<T>: IRepository<T> where T : class
{
    protected readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    protected IQueryable<T> GetAllQueryable()
    {
        return _context.Set<T>().AsNoTracking();
    }
    
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        // SET do EF acessa uma coleção de uma tabela 
        // EX: _context.Produtos = _context.Set<Produto>()
        return await GetAllQueryable().ToListAsync();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(predicate);
    }

    public T Create(T entity)
    {
        _context.Set<T>().Add(entity);
        //_context.SaveChanges();
        return entity;
    }

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        //_context.SaveChanges();
        return entity;
    }

    public T Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        //_context.SaveChanges();
        return entity;
    }
}
