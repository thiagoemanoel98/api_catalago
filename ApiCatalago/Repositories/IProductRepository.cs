using ApiCatalago.Models;
using ApiCatalago.Pagination;

namespace ApiCatalago.Repositories;

public interface IProductRepository : IRepository<Product>
{
    //IEnumerable<Product> GetProducts(ProductsParameters productsParameters);
    
    Task<PagedList<Product>> GetProductsAsync(ProductsParameters productsParameters);
    Task<PagedList<Product>> GetProductsPriceFilterAsync(ProductsFilterPrice productsFilterPriceParameters);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(int id);
}
