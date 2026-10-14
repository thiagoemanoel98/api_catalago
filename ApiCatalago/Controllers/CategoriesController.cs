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
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCate(
        [FromQuery] CategoriesParameters parameters)
    {
        var categories = await _uof.CategoryRepository.GetCategoriesAsync(parameters);

        return GetCategories(categories);
    }

    private ActionResult<IEnumerable<CategoryDTO>> GetCategories(PagedList<Category> categories)
    {
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

    [HttpGet("filter/name/pagination")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesFiltered(
        [FromQuery] CategoriesFilterName categoriesFilterName)
    {
        var categoriesFiltered = await _uof.CategoryRepository.GetCategoriesNameFilterAsync(categoriesFilterName);

        return GetCategories(categoriesFiltered);
    }
    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
    {
        var categories = await _uof.CategoryRepository.GetAllAsync();

        if (categories is null)
            return NotFound();

        var categoriesDto = categories.ToCategoryDtoList();
        
        return Ok(categoriesDto); 
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public async Task<ActionResult<CategoryDTO>> Get(int id)
    {
        var category = await _uof.CategoryRepository.GetAsync(c => c.CategoryId == id);

        if (category is null)
        {
            _logger.LogInformation("Erro get category by id");
            return NotFound("Caregoria não encontrada");
        }
        
        var categoryDto = category.ToCategoryDto();
        
        return Ok(categoryDto);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDTO>> Post(CategoryDTO categoryDto)
    {
        if (categoryDto is null)
        {
            _logger.LogInformation("Dados inválidos");
            return BadRequest("Dados inválidos");
        }
        
        var category = categoryDto.ToCategory();

        var categoryCreated = _uof.CategoryRepository.Create(category);
        await _uof.CommitAsync();

        var newCategoryDto = categoryCreated.ToCategoryDto();

        return new CreatedAtRouteResult("GetCategory", 
            new { id = newCategoryDto.CategoryId }, newCategoryDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoryDTO>> Put(int id, CategoryDTO categoryDto)
    {
        if (id != categoryDto.CategoryId)
        {
            return BadRequest();
        }

        var category = categoryDto.ToCategory();
        
        var categoryUpdated = _uof.CategoryRepository.Update(category);
        await _uof.CommitAsync();

        var categoryUpdatedDto = categoryUpdated.ToCategoryDto();
        
        return Ok(categoryUpdatedDto );
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var category = await _uof.CategoryRepository.GetAsync(c => c.CategoryId == id);

        if (category is null)
        {
            _logger.LogInformation("Categoria não encontrada");
            return NotFound("Categoria não encontrada");
        }

        var categoryDeleted = _uof.CategoryRepository.Delete(category);
        await _uof.CommitAsync();

        var categoryDeletedDto = categoryDeleted.ToCategoryDto();
        
        return Ok(categoryDeletedDto);
    }
    
}
