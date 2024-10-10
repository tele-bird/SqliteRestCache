using AutoMapper;
using BethanysPieShop.API.DTO;
using BethanysPieShop.API.Entities;
using BethanysPieShop.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShop.API.Controllers;

[Route("api/[controller]")]
public class PiesController : Controller
{
    private readonly IPieService _pieService;
    private readonly IMapper _mapper;

    public PiesController(
        IMapper mapper, IPieService pieService)
    {
        _mapper = mapper;
        _pieService = pieService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<PieDto>>> GetAllPies()
    {
        List<Pie> pies = await _pieService.GetAllPies();
        var mappedPies = _mapper.Map<List<PieDto>>(pies);
        return Ok(mappedPies);
    }
    
    [HttpGet("piesoftheweek")]
    public async Task<ActionResult<List<PieDto>>> GetPiesOfTheWeek()
    {
        List<Pie> pies = await _pieService.GetPiesOfTheWeek();
        var mappedPies = _mapper.Map<List<PieDto>>(pies);
        return Ok(mappedPies);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<PieDetailDto>> GetPieById(int id)
    {
        Pie? pie = await _pieService.GetPieById(id);
        if (pie is null)
        {
            return NotFound();
        }
        
        var mappedPie = _mapper.Map<PieDetailDto>(pie);
        return Ok(mappedPie);
    }
}