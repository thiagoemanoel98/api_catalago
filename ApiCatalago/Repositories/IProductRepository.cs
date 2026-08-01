using ApiCatalago.Models;

namespace ApiCatalago.Repositories;

public interface IProductRepository : IRepository<Product>
{
    IEnumerable<Product> GetProductsByCategory(int id);
}