using ApiCatalago.Context;
using ApiCatalago.Filters;
using ApiCatalago.Models;
using ApiCatalago.Repositories;
using ApiCatalago.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriesController: ControllerBase
{
    private readonly ICategoryRepository _repository;
    private readonly ILogger _logger;
    
    public CategoriesController(ICategoryRepository repository, ILogger<CategoriesController> logger) 
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<Category>> Get()
    {
        var categories = _repository.GetCategories();
        return Ok(categories);
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public ActionResult<Category> Get(int id)
    {
        var category = _repository.GetCategory(id);

        if (category is null)
        {
            _logger.LogInformation("Erro get category by id");
            return NotFound("Caregoria não encontrada");
        }
        return Ok(category);
    }

    [HttpPost]
    public ActionResult Post(Category category)
    {
        if (category is null)
        {
            _logger.LogInformation("Dados inválidos");
            return BadRequest("Dados inválidos");
        }

        var categoryCreated = _repository.Create(category);

        return new CreatedAtRouteResult("GetCategory", new { id = categoryCreated.CategoryId }, categoryCreated);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Category category)
    {
        if (id != category.CategoryId)
        {
            return BadRequest();
        }

        _repository.Update(category);
        return Ok(category);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var category = _repository.GetCategory(id);

        if (category is null)
        {
            _logger.LogInformation("Categoria não encontrada");
            return NotFound("Categoria não encontrada");
        }

        var categoryDeleted = _repository.Delete(id);
        return Ok(categoryDeleted);
    }
    
}