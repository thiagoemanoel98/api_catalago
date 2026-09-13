using ApiCatalago.Context;
using ApiCatalago.Models;
using ApiCatalago.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
    
    public PagedList<Category> GetCategories(CategoriesParameters parameters)
    {
        var categories = GetAll().OrderBy(c => c.CategoryId).AsQueryable();
        var categoriesOrdered =
            PagedList<Category>.ToPagedList(categories, parameters.PageNumber, parameters.PageSize);
        return categoriesOrdered; 
    }

}