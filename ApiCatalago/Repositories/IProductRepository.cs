using ApiCatalago.Models;
using ApiCatalago.Pagination;

namespace ApiCatalago.Repositories;

public interface IProductRepository : IRepository<Product>
{
    IEnumerable<Product> getProducts(ProductsParameters productsParameters);
    IEnumerable<Product> GetProductsByCategory(int id);
}