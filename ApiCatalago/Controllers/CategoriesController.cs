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
    //private readonly IRepository<Category> _repository;
    private readonly IUnitOfWork _uof; 
    private readonly ILogger<CategoriesController> _logger;
    
    public CategoriesController(ICategoryRepository repository, ILogger<CategoriesController> logger, IUnitOfWork uof)
    {
        _logger = logger;
        _uof = uof;
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<Category>> Get()
    {
        var categories = _uof.CategoryRepository.GetAll();
        return Ok(categories);
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public ActionResult<Category> Get(int id)
    {
        var category = _uof.CategoryRepository.Get(c => c.CategoryId == id);

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

        var categoryCreated = _uof.CategoryRepository.Create(category);
        _uof.Commit();

        return new CreatedAtRouteResult("GetCategory", new { id = categoryCreated.CategoryId }, categoryCreated);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Category category)
    {
        if (id != category.CategoryId)
        {
            return BadRequest();
        }

        _uof.CategoryRepository.Update(category);
        _uof.Commit();
        return Ok(category);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var category = _uof.CategoryRepository.Get(c => c.CategoryId == id);

        if (category is null)
        {
            _logger.LogInformation("Categoria não encontrada");
            return NotFound("Categoria não encontrada");
        }

        var categoryDeleted = _uof.CategoryRepository.Delete(category);
        _uof.Commit();
        return Ok(categoryDeleted);
    }
    
}