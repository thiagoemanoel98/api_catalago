using ApiCatalago.Context;
using ApiCatalago.DTOs;
using ApiCatalago.Models;
using ApiCatalago.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController: ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;
        
    public ProductsController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet("products/{id}")]
    public ActionResult <IEnumerable<ProductDTO>> GetProductsByCategory(int id)
    {
       var products = _uof.ProductRepository.GetProductsByCategory(id);

        if(products is null)
            return NotFound();

        var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products); 
        
        return Ok(productsDto);
    }

    [HttpGet()]
    public ActionResult<IEnumerable<ProductDTO>> GetProducts()
    {
        var products = _uof.ProductRepository.GetAll();
        if (products is null)
        {
            return NotFound();
        }
        
        var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products); 
        
        return Ok(productsDto);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
    public ActionResult<ProductDTO> Get(int id)
    {
        var product = _uof.ProductRepository.Get(p => p.ProductId == id);
        if (product is null)
            return NotFound("Produto não encontrado");

        var productDto = _mapper.Map<ProductDTO>(product); 
        return Ok(productDto);
    }

    [HttpPost]
    public ActionResult<ProductDTO> Post(ProductDTO productDto)
    {
        if (productDto is null)
            return BadRequest();

        var product = _mapper.Map<Product>(productDto);
        
        var newProduct = _uof.ProductRepository.Create(product);
        _uof.Commit();

        var newProductDto = _mapper.Map<ProductDTO>(product);
        
        return new CreatedAtRouteResult("GetProduct", new { id = newProductDto.ProductId }, newProductDto);
    }


    [HttpPatch("{id}/UpdatePartial")]
    public ActionResult<ProductDTOUpdateResponse> Patch(int id, 
        JsonPatchDocument<ProductDTOUpdateRequest> patchProductDto)
    {
        if(patchProductDto is null || id <= 0)
            return BadRequest();
        var product = _uof.ProductRepository.Get(p => p.ProductId == id);

        if (product is null)
            return NotFound();

        // Aplica as alterações parciais
        var productUpdateRequest = _mapper.Map<ProductDTOUpdateRequest>(product);
        patchProductDto.ApplyTo(productUpdateRequest, ModelState);

        if (!ModelState.IsValid || !TryValidateModel(productUpdateRequest))
            return BadRequest(ModelState);

        // Salva no banco de dados
        _mapper.Map(productUpdateRequest, product);
        _uof.ProductRepository.Update(product);
        _uof.Commit();
        
        return Ok(_mapper.Map<ProductDTOUpdateResponse>(product)); 
    } 
    
    
    [HttpPut("{id:int}")]
    public ActionResult<ProductDTO> Put(int id, ProductDTO productDto)
    {
        if (id != productDto.ProductId)
        {
            return BadRequest();
        }
        
        var product = _mapper.Map<Product>(productDto);

        var productUpdated = _uof.ProductRepository.Update(product);
        _uof.Commit();
        
        var productUpdatedDto = _mapper.Map<ProductDTO>(productUpdated);
        
        return Ok(productUpdatedDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ProductDTO> Delete(int id)
    {
        var product = _uof.ProductRepository.Get(p => p.ProductId == id);

        if(product is null)
        {
            return NotFound("Produto não encontrado...");
        }

        var productDeleted = _uof.ProductRepository.Delete(product);
        _uof.Commit();
        
        return Ok(productDeleted);
    }
}