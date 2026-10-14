using ApiCatalago.Models;
using ApiCatalago.Pagination;
using X.PagedList;

namespace ApiCatalago.Repositories;

public interface IProductRepository : IRepository<Product>
{
    //IEnumerable<Product> GetProducts(ProductsParameters productsParameters);
    
    Task<IPagedList<Product>> GetProductsAsync(ProductsParameters productsParameters);
    Task<IPagedList<Product>> GetProductsPriceFilterAsync(ProductsFilterPrice productsFilterPriceParameters);
    Task<IPagedList<Product>> GetProductsByCategoryAsync (int id);
}