using ApiCatalago.Context;
using ApiCatalago.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalago.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController: ControllerBase
{
    // Injetação AppDbContext
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    // IENumerable mais otimizado que o List nesse caso
    [HttpGet]
    public ActionResult<IEnumerable<Product>> Get()
    {
        var products = _context.Products.ToList();
        if (products is null)
        {
            return NotFound();
        }
        
        return products;
    }
}