using ApiCatalago.Context;
using ApiCatalago.Models;
using ApiCatalago.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }
    
    public Task<PagedList<Product>> GetProductsAsync(ProductsParameters productsParameters)
    {
        var products = GetAllQueryable()
            .OrderBy(product => product.ProductId);

        return PagedList<Product>.ToPagedListAsync(
            products,
            productsParameters.PageNumber,
            productsParameters.PageSize);
    }

    public Task<PagedList<Product>> GetProductsPriceFilterAsync(ProductsFilterPrice productsFilterPriceParameters)
    {
        IQueryable<Product> products = GetAllQueryable();

        if (productsFilterPriceParameters.Price.HasValue &&
            !string.IsNullOrEmpty(productsFilterPriceParameters.PriceCriterion))
        {
             if(productsFilterPriceParameters.PriceCriterion.Equals("maior", StringComparison.OrdinalIgnoreCase))
             {
                 products = products.Where(product => product.Price > productsFilterPriceParameters.Price.Value);
             }
             else if(productsFilterPriceParameters.PriceCriterion.Equals("menor", StringComparison.OrdinalIgnoreCase))
             {
                 products = products.Where(product => product.Price < productsFilterPriceParameters.Price.Value);
             }
             else if(productsFilterPriceParameters.PriceCriterion.Equals("igual", StringComparison.OrdinalIgnoreCase))
             {
                 products = products.Where(product => product.Price == productsFilterPriceParameters.Price.Value);
             }
        }

        products = products.OrderBy(product => product.Price);

        return PagedList<Product>.ToPagedListAsync(
            products,
            productsFilterPriceParameters.PageNumber,
            productsFilterPriceParameters.PageSize);
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int id)
    {
        return await GetAllQueryable()
            .Where(product => product.CategoryId == id)
            .OrderBy(product => product.ProductId)
            .ToListAsync();
    }
}
