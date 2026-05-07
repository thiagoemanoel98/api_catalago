using ApiCatalago.Context;
using ApiCatalago.Models;
using ApiCatalago.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController: ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet()]
    public ActionResult<IEnumerable<Product>> GetProducts()
    {
        var products = _repository.GetProducts().ToList();
        if (products is null)
        {
            return NotFound();
        }
        
        return Ok(products);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
    public ActionResult<Product> Get(int id)
    {
        var product = _repository.GetProduct(id);
        if (product is null)
            return NotFound("Produto não encontrado");
        return Ok(product);
    }

    [HttpPost]
    public ActionResult Post(Product product)
    {
        if (product is null)
            return BadRequest();

        var newProduct = _repository.Create(product);
        
        return new CreatedAtRouteResult("GetProduct", new { id = newProduct.ProductId }, newProduct);
    }

    // Restrição: Valor tem que ser inteiro 
    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Product product)
    {
        if (id != product.ProductId)
        {
            return BadRequest();
        }

        bool updated = _repository.Update(product);

        if (updated)
            return Ok(product);
      
        return StatusCode(500, $"Falha ao atualizar o produto de id = {id}");
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        bool deleted = _repository.Delete(id);
        if (deleted)
        {
            return Ok($"Produto id={id} foi excluido ");
        }
        return StatusCode(500, "Falha ao excluir produto");
    }
}