using ApiCatalago.Context;
using ApiCatalago.Models;
using ApiCatalago.Pagination;

namespace ApiCatalago.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    /** public IEnumerable<Product> getProducts(ProductsParameters productsParameters)
     {
         return GetAll()
             .OrderBy(p => p.Name)
             .Skip((productsParameters.PageNumber - 1) * productsParameters.PageSize)
             .Take(productsParameters.PageSize).ToList();

     }*/

    public PagedList<Product> GetProducts(ProductsParameters productsParameters)
    {
        var products = GetAll().OrderBy(p => p.ProductId).AsQueryable();
        var productsOrdered =
            PagedList<Product>.ToPagedList(products, productsParameters.PageNumber, productsParameters.PageSize);
        return productsOrdered;
    }

    public PagedList<Product> GetProductsPriceFilter(ProductsFilterPrice productsFilterPriceParameters)
    {
        var products = GetAll().AsQueryable();

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

        var productsFiltered = PagedList<Product>.ToPagedList(products, productsFilterPriceParameters.PageNumber,
            productsFilterPriceParameters.PageSize);
        return productsFiltered;
    }

    public IEnumerable<Product> GetProductsByCategory(int id)
    {
        return GetAll().Where(c => c.CategoryId == id);
    }
}
