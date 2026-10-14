using ApiCatalago.Context;
using ApiCatalago.Models;
using ApiCatalago.Pagination;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace ApiCatalago.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
    
    public async Task<IPagedList<Category>> GetCategoriesAsync(CategoriesParameters parameters)
    {
        var categories = await GetAllAsync();
        var categoriesOrdered = categories.OrderBy(c => c.CategoryId).AsQueryable();
        
        var result = Pagination.PagedList<Category>.ToPagedList(categoriesOrdered, parameters.PageNumber, parameters.PageSize);
        
        return result; 
    }

    public async Task<IPagedList<Category>> GetCategoriesNameFilterAsync(CategoriesFilterName parameters)
    {
        var categories = await GetAllAsync();

        if (!string.IsNullOrEmpty(parameters.Name))
        {
            categories = categories.Where(c => c.Name.Contains(parameters.Name));
        }

        //var categoriesFiltered =
        //    Pagination.PagedList<Category>.ToPagedList(categories.AsQueryable(), parameters.PageNumber, parameters.PageSize);
        var categoriesFiltered = await categories.ToPagedListAsync(parameters.PageNumber, parameters.PageSize);
        
        return categoriesFiltered;
    }
}