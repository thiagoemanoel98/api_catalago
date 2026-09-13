using ApiCatalago.Models;
using ApiCatalago.Pagination;

namespace ApiCatalago.Repositories;

public interface IProductRepository : IRepository<Product>
{
    //IEnumerable<Product> GetProducts(ProductsParameters productsParameters);
    
    PagedList<Product> GetProducts(ProductsParameters productsParameters);
    
    IEnumerable<Product> GetProductsByCategory(int id);
}