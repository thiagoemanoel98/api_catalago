using ApiCatalago.Context;
using ApiCatalago.Models;
using ApiCatalago.Pagination;

namespace ApiCatalago.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
    
    public Task<PagedList<Category>> GetCategoriesAsync(CategoriesParameters parameters)
    {
        var categories = GetAllQueryable()
            .OrderBy(category => category.CategoryId);

        return PagedList<Category>.ToPagedListAsync(
            categories,
            parameters.PageNumber,
            parameters.PageSize);
    }

    public Task<PagedList<Category>> GetCategoriesNameFilterAsync(CategoriesFilterName parameters)
    {
        IQueryable<Category> categories = GetAllQueryable();

        if (!string.IsNullOrEmpty(parameters.Name))
        {
            categories = categories.Where(category =>
                category.Name != null && category.Name.Contains(parameters.Name));
        }

        categories = categories.OrderBy(category => category.CategoryId);

        return PagedList<Category>.ToPagedListAsync(
            categories,
            parameters.PageNumber,
            parameters.PageSize);
    }
}
