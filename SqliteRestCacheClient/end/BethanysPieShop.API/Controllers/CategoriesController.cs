using AutoMapper;
using BethanysPieShop.API.DTO;
using BethanysPieShop.API.Entities;
using BethanysPieShop.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShop.API.Controllers;

[Route("api/[controller]")]
public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public CategoriesController(
        ICategoryService categoryService, 
        IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetAllCategories()
    {
        List<Category> categories = await _categoryService.GetCategories();
        var mappedCategories = _mapper.Map<List<CategoryDto>>(categories);
        return Ok(mappedCategories);
    }
}