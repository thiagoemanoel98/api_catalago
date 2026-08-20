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
    private readonly IProductRepository _productRepository; // Especifico: para uso especifico
    private readonly IRepository<Product> _repository;


    public ProductsController(IRepository<Product> repository ,IProductRepository productRepository)
    {
        _repository = repository;
        _productRepository = productRepository;
    }

    [HttpGet("products/{id}")]
    public ActionResult <IEnumerable<Product>> GetProductsByCategory(int id)
    {
       var products = _productRepository.GetProductsByCategory(id);

        if(products is null)
            return NotFound();
        
        return Ok(products);
    }

    [HttpGet()]
    public ActionResult<IEnumerable<Product>> GetProducts()
    {
        var products = _repository.GetAll();
        if (products is null)
        {
            return NotFound();
        }
        
        return Ok(products);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
    public ActionResult<Product> Get(int id)
    {
        var product = _repository.Get(p => p.ProductId == id);
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

        var productUpdated = _repository.Update(product);

        return Ok(productUpdated);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var product = _repository.Get(p => p.ProductId == id);

        if(product is null)
        {
            return NotFound("Produto não encontrado...");
        }

        var productDeleted = _repository.Delete(product);

        return Ok(productDeleted);
    }
}