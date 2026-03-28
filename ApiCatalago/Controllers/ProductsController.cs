using ApiCatalago.Context;
using ApiCatalago.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<ActionResult<IEnumerable<Product>>> Get()
    {
        var products = await _context.Products.AsNoTracking().ToListAsync();
        if (products is null)
        {
            return NotFound();
        }
        
        return products;
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<Product>> Get(int id)
    {
        var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.ProductId == id);
        if (product is null)
            return NotFound("Produto não encontrado");
        return product;
    }

    [HttpPost]
    public ActionResult Post(Product product)
    {
        if (product is null)
            return BadRequest();
        
        _context.Products.Add(product);
        _context.SaveChanges();
        
        return new CreatedAtRouteResult("Get", new { id = product.ProductId }, product);
    }

    // Restrição: Valor tem que ser inteiro 
    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Product product)
    {
        if (id != product.ProductId)
        {
            return BadRequest();
        }

        // EF Core vai saber que essa entidade vai ser alterada e persistida
        _context.Entry(product).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

        if (product is null)
        {
            return NotFound("Produto não localizado");
        }

        _context.Products.Remove(product);
        _context.SaveChanges();

        return Ok(product);
    }
}