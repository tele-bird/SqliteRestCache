using System.Diagnostics;
using System.Text.Json;
using AutoMapper;
using BethanysPieShop.API.DTO;
using BethanysPieShop.API.Entities;
using BethanysPieShop.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShop.API.Controllers;

[Route("api/[controller]")]
public class ShoppingCartController : Controller
{
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;


    public ShoppingCartController(
        IShoppingCartService shoppingCartService,
        IMapper mapper, UserManager<IdentityUser> userManager)
    {
        _shoppingCartService = shoppingCartService;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShoppingCartDto>> GetShoppingCartById(long id)
    {
        var shoppingCartEntity = await _shoppingCartService.GetById(id);
        if (shoppingCartEntity is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ShoppingCartDto>(shoppingCartEntity));
    }

    [HttpGet]
    public async Task<ActionResult<ShoppingCartDto>> GetShoppingCartForUser()
    {
        ShoppingCart? shoppingCartEntity;
        if (!HttpContext.User?.Identity?.IsAuthenticated ?? false)
        {
            shoppingCartEntity = await _shoppingCartService.CreateShoppingCart();
        }
        else
        {
            var userId = (await _userManager.GetUserAsync(HttpContext.User!))!.Id;
            shoppingCartEntity = await _shoppingCartService.GetByUserId(userId);
            if (shoppingCartEntity is null)
            {
                return NotFound();
            }
        }

        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<long>> CreateShoppingCart()
    {
        string? userId = null;
        if (HttpContext.User?.Identity?.IsAuthenticated ?? false)
        {
            userId = (await _userManager.GetUserAsync(HttpContext.User))!.Id;
        }

        var cart = await _shoppingCartService.CreateShoppingCart(userId);

        return Ok(cart.Id);
    }

    [HttpPost("{id}/add")]
    public async Task<ActionResult<long>> AddShoppingCartItem([FromRoute] long id, [FromBody] ShoppingCartItemDto item)
    {
        ArgumentNullException.ThrowIfNull(item.PieId);

        Debug.WriteLine("DEBUG ADD: " + JsonSerializer.Serialize(item));

        string? userId = null;
        if (HttpContext.User?.Identity?.IsAuthenticated ?? false)
        {
            userId = (await _userManager.GetUserAsync(HttpContext.User))!.Id;
        }

        var shoppingCartItem = new ShoppingCartItem
        {
            PieId = item.PieId,
            Quantity = item.Quantity,
            ShoppingCartId = id
        };

        await _shoppingCartService.AddItem(shoppingCartItem, id, userId);
        await _shoppingCartService.SaveAsync();

        return Ok(shoppingCartItem.Id);
    }

    [HttpPatch("{id}/item/{itemId}")]
    public async Task<IActionResult> UpdateShoppingCartItemQuantity([FromRoute] long id, [FromRoute] long itemId,
        [FromBody] ShoppingCartItemDto item)
    {
        if (item.Id != itemId)
        {
            throw new ArgumentException();
        }

        Debug.WriteLine("DEBUG UPDATE: "+ JsonSerializer.Serialize(item));

        string? userId = null;
        if (HttpContext.User?.Identity?.IsAuthenticated ?? false)
        {
            userId = (await _userManager.GetUserAsync(HttpContext.User))!.Id;
        }

        int quantity = item.Quantity;

        await _shoppingCartService.UpdateItemQuantity(id, itemId, quantity, userId);
        await _shoppingCartService.SaveAsync();

        return Ok();
    }

    [HttpDelete("{id}/item/{itemId}")]
    public async Task<IActionResult> DeleteShoppingCartItem([FromRoute] long id, [FromRoute] long itemId)
    {
        await _shoppingCartService.DeleteItem(itemId, id);
        await _shoppingCartService.SaveAsync();

        return Ok();
    }

    [HttpDelete("{id}/all")]
    public async Task<IActionResult> ClearShoppingCart([FromRoute] long id)
    {
        var shoppingCartEntity = await _shoppingCartService.GetById(id);
        if (shoppingCartEntity?.Items != null)
            foreach (var item in shoppingCartEntity?.Items)
            {
                await _shoppingCartService.DeleteItem(item!.Id, id);
            }

        await _shoppingCartService.SaveAsync();

        return Ok();
    }
}