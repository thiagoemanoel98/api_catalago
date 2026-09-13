using ApiCatalago.Models;
using ApiCatalago.Pagination;

namespace ApiCatalago.Repositories;

public interface ICategoryRepository: IRepository<Category>
{
    PagedList<Category> GetCategories(CategoriesParameters  parameters);
}