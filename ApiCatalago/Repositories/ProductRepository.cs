using ApiCatalago.Context;
using ApiCatalago.Models;
using ApiCatalago.Pagination;
using X.PagedList;
using X.PagedList.Extensions;

namespace ApiCatalago.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }
    
    public async Task<IPagedList<Product>> GetProductsAsync(ProductsParameters productsParameters)
    {
        var products = await GetAllAsync();
        var productsOrdered = products.OrderBy(p => p.ProductId).AsQueryable();
        
        var result = await
            productsOrdered.ToPagedList(productsOrdered, productsParameters.PageNumber, productsParameters.PageSize);
        return result ;
    }

    public async Task<Pagination.PagedList<Product>> GetProductsPriceFilterAsync(ProductsFilterPrice productsFilterPriceParameters)
    {
        var products = await GetAllAsync();

        if (productsFilterPriceParameters.Price.HasValue &&
            !string.IsNullOrEmpty(productsFilterPriceParameters.PriceCriterion))
        {
             if(productsFilterPriceParameters.PriceCriterion.Equals("maior", StringComparison.OrdinalIgnoreCase))
             {
                 products = products.Where(p => p.Price > productsFilterPriceParameters.Price.Value).
                     OrderBy(p=>p.Price);
             }
             else if(productsFilterPriceParameters.PriceCriterion.Equals("menor", StringComparison.OrdinalIgnoreCase))
             {
                 products = products.Where(p => p.Price < productsFilterPriceParameters.Price.Value).
                     OrderBy(p=>p.Price);
             }
             else if(productsFilterPriceParameters.PriceCriterion.Equals("igual", StringComparison.OrdinalIgnoreCase))
             {
                 products = products.Where(p => p.Price == productsFilterPriceParameters.Price.Value).
                     OrderBy(p=>p.Price);
             }
        }

        var productsFiltered = Pagination.PagedList<Product>.ToPagedList(products.AsQueryable(), productsFilterPriceParameters.PageNumber,
            productsFilterPriceParameters.PageSize);
        return productsFiltered;
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int id)
    {
        var products = await GetAllAsync();
        var productsCategory = products.Where(p => p.CategoryId == id);
        return productsCategory; 
    }
}
