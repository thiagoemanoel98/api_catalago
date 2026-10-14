using ApiCatalago.Models;
using ApiCatalago.Pagination;

namespace ApiCatalago.Repositories;

public interface ICategoryRepository: IRepository<Category>
{
    Task<PagedList<Category>> GetCategoriesAsync(CategoriesParameters parameters);
    Task<PagedList<Category>> GetCategoriesNameFilterAsync(CategoriesFilterName parameters);
}
