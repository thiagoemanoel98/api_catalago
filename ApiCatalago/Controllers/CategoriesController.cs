using ApiCatalago.Context;
using ApiCatalago.Filters;
using ApiCatalago.Models;
using ApiCatalago.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriesController: ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger _logger;
    
    public CategoriesController(AppDbContext context, ILogger<CategoriesController> logger) 
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("UseFromServices/{name}")]
    public ActionResult<string> GetTalkFromServices([FromServices] IMyService myService, string name)
    {
        return myService.Talk(name);
    }
    
    [HttpGet("NoUseFromServices/{name}")]
    public ActionResult<string> GetTalkNoFromServices(IMyService myService, string name)
    {
        return myService.Talk(name);
    }
    

    // categories/products
    [HttpGet("products")]
    public ActionResult<IEnumerable<Category>> GetCategoriesProducts()
    {
        _logger.LogInformation($" ======== GET api/categories/products ======== ");

        return _context.Categories.Include(c => c.Products).ToList<Category>();
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<Category>> Get()
    {
        return _context.Categories.AsNoTracking().ToList();
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public ActionResult<Category> Get(int id)
    {
       
        var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);

        if (category is null)
        {
            return NotFound("Caregoria não encontrada");
        }
        return Ok(category);
    }

    [HttpPost]
    public ActionResult Post(Category category)
    {
        if (category is null)
        {
            return BadRequest("Dados inválidos");
        }

        _context.Categories.Add(category);
        _context.SaveChanges();

        return new CreatedAtRouteResult("GetCategory", new { id = category.CategoryId }, category);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Category category)
    {
        if (id != category.CategoryId)
        {
            return BadRequest();
        }

        _context.Entry(category).State = EntityState.Modified;
        _context.SaveChanges();
        return Ok(category);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);

        if (category is null)
        {
            return BadRequest();
        }

        _context.Categories.Remove(category);
        _context.SaveChanges();
        return Ok(category);
    }
    
}