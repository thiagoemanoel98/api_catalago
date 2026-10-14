using ApiCatalago.Models;
using ApiCatalago.Pagination;
using X.PagedList;

namespace ApiCatalago.Repositories;

public interface ICategoryRepository: IRepository<Category>
{
    Task<IPagedList<Category>> GetCategoriesAsync(CategoriesParameters  parameters);
    Task<IPagedList<Category>> GetCategoriesNameFilterAsync(CategoriesFilterName parameters);
}