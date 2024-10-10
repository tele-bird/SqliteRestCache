using AutoMapper;
using BethanysPieShop.API.DTO;
using BethanysPieShop.API.Entities;
using BethanysPieShop.API.Repositories.Interfaces;
using BethanysPieShop.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShop.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class OrdersController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;

    public OrdersController(
        IMapper mapper, 
        UserManager<IdentityUser> userManager, 
        IOrderService orderService)
    {
        _mapper = mapper;
        _userManager = userManager;
        _orderService = orderService;
    }
    
    [HttpGet("{id}", Name = "GetOrder")]
    public async Task<ActionResult<OrderDto>> GetOrderById(long id)
    {
        var orderEntity = await _orderService.GetOrderById(id);
        if (orderEntity is null)
        {
            return NotFound();
        }
        
        return Ok(_mapper.Map<OrderDto>(orderEntity));
    }
    
    [HttpGet]
    public async Task<ActionResult<OrderDto>> GetOrderHistory()
    {
        var userUuid = (await _userManager.GetUserAsync(HttpContext.User))!.Id;
        List<Order> orderEntities = await _orderService.GetOrdersForUser(userUuid);

        return Ok(_mapper.Map<List<OrderDto>>(orderEntities));
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto order)
    {
        string userId = (await _userManager.GetUserAsync(HttpContext.User))!.Id;
        
        var orderEntity = _mapper.Map<Order>(order);

        _orderService.AddOrder(orderEntity, userId);
        await _orderService.SaveAsync();

        return CreatedAtRoute("GetOrder", new { id = orderEntity.Id }, _mapper.Map<OrderDto>(orderEntity));
    }
}