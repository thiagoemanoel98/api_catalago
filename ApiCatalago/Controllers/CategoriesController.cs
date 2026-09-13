using ApiCatalago.Context;
using ApiCatalago.DTOs;
using ApiCatalago.DTOs.Mappings;
using ApiCatalago.Filters;
using ApiCatalago.Models;
using ApiCatalago.Pagination;
using ApiCatalago.Repositories;
using ApiCatalago.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ApiCatalago.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriesController: ControllerBase
{
    //private readonly IRepository<Category> _repository;
    private readonly IUnitOfWork _uof; 
    private readonly ILogger<CategoriesController> _logger;
    private readonly IMapper _mapper;

    
    public CategoriesController(ICategoryRepository repository, ILogger<CategoriesController> logger, IUnitOfWork uof, IMapper mapper)
    {
        _logger = logger;
        _uof = uof;
        _mapper = mapper;
    }
    
    [HttpGet("pagination")]
    public ActionResult<IEnumerable<CategoryDTO>> GetCategories(
        [FromQuery] CategoriesParameters parameters)
    {
        var categories = _uof.CategoryRepository.GetCategories(parameters);

        var metaData = new
        {
            categories.TotalCount,
            categories.PageSize,
            categories.CurrentPage,
            categories.TotalPages,
            categories.HasNext,
            categories.HasPrevius
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData)); 
        
        var categoriesDto = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        
        return Ok(categoriesDto);
    } 

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<CategoryDTO>> Get()
    {
        var categories = _uof.CategoryRepository.GetAll();

        if (categories is null)
            return NotFound();

        var categoriesDto = categories.ToCategoryDtoList();
        
        return Ok(categoriesDto); 
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public ActionResult<CategoryDTO> Get(int id)
    {
        var category = _uof.CategoryRepository.Get(c => c.CategoryId == id);

        if (category is null)
        {
            _logger.LogInformation("Erro get category by id");
            return NotFound("Caregoria não encontrada");
        }
        
        var categoryDto = category.ToCategoryDto();
        
        return Ok(categoryDto);
    }

    [HttpPost]
    public ActionResult<CategoryDTO> Post(CategoryDTO categoryDto)
    {
        if (categoryDto is null)
        {
            _logger.LogInformation("Dados inválidos");
            return BadRequest("Dados inválidos");
        }
        
        var category = categoryDto.ToCategory();

        var categoryCreated = _uof.CategoryRepository.Create(category);
        _uof.Commit();

        var newCategoryDto = categoryCreated.ToCategoryDto();

        return new CreatedAtRouteResult("GetCategory", 
            new { id = newCategoryDto.CategoryId }, newCategoryDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoryDTO> Put(int id, CategoryDTO categoryDto)
    {
        if (id != categoryDto.CategoryId)
        {
            return BadRequest();
        }

        var category = categoryDto.ToCategory();
        
        var categoryUpdated = _uof.CategoryRepository.Update(category);
        _uof.Commit();

        var categoryUpdatedDto = categoryUpdated.ToCategoryDto();
        
        return Ok(categoryUpdatedDto );
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

        var categoryDeletedDto = categoryDeleted.ToCategoryDto();
        
        return Ok(categoryDeletedDto);
    }
    
}