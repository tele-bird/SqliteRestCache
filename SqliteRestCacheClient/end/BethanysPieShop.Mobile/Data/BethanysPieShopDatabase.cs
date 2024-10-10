using System.Runtime.CompilerServices;
using SQLite;
using BethanysPieShop.Mobile.Helpers;
using BethanysPieShop.Mobile.Models;
using Newtonsoft.Json;

namespace BethanysPieShop.Mobile.Data;

public class BethanysPieShopDatabase
{
    SQLiteAsyncConnection? _database;

    public BethanysPieShopDatabase()
    {
    }

    public async Task Init()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await _database.CreateTableAsync<ShoppingCartItem>();

        await ClearShoppingCart();
    }

    public async Task<long?> GetShoppingCartId()
    {
        await Init();

        var item = await _database!.Table<ShoppingCartItem>()
            .OrderByDescending(i => i.ItemId)
            .FirstOrDefaultAsync();

        return item?.CartId;
    }

    public async Task AddItem(ShoppingCartItemModel shoppingCartItem, long? cartId, ActionTypeEnum actionTypeId = ActionTypeEnum.None)
    {
        await Init();

        var itemDB = await _database!.Table<ShoppingCartItem>()
            .FirstOrDefaultAsync(i => i.PieId == shoppingCartItem.PieId);

        if (itemDB == null)
        {
            ShoppingCartItem item = new ShoppingCartItem
            {
                CartId = cartId,
                ImageThumbnailUrl = shoppingCartItem.Pie.ImageThumbnailUrl,
                Id = shoppingCartItem.Id,
                Name = shoppingCartItem.Pie.Name,
                PieId = shoppingCartItem.Pie.Id,
                Price = shoppingCartItem.Pie.Price,
                Quantity = shoppingCartItem.Quantity,
                ActionId = actionTypeId
            };

            await _database!.InsertAsync(item);
        }
        else
        {
            await UpdateItem(shoppingCartItem, ActionTypeEnum.Updated);
        }
        
    }

    public async Task ClearShoppingCart(ActionTypeEnum actionTypeId = ActionTypeEnum.None)
    {
        await Init();

        var items = await _database!.Table<ShoppingCartItem>()
            .ToListAsync();

        foreach (var item in items)
        {

            if (actionTypeId != ActionTypeEnum.Deleted)
            {
                await _database.DeleteAsync(item);
            }
            else
            {
                var updatedItem = CloneShoppingCartItem(actionTypeId, item);

                await _database.UpdateAsync(updatedItem);
            }
        }
    }

    

    public async Task DeleteItem(int id, ActionTypeEnum actionTypeId = ActionTypeEnum.None)
    {
        await Init();

        var item = await _database!.Table<ShoppingCartItem>()
            .FirstOrDefaultAsync(i => i.Id == id);

        if (item is not null)
        {
            if (actionTypeId != ActionTypeEnum.Deleted)
            {
                await _database.DeleteAsync(item);
            }
            else
            {
                var itemToUpdate = CloneShoppingCartItem(actionTypeId, item);
                await _database.UpdateAsync(itemToUpdate);
            }
        }
    }

    public async Task UpdateItem(ShoppingCartItemModel shoppingCartItem, ActionTypeEnum actionTypeId = ActionTypeEnum.None)
    {
        await Init();

        var itemDB = await _database!.Table<ShoppingCartItem>()
            .FirstOrDefaultAsync(i => i.PieId == shoppingCartItem.PieId);

        if (itemDB is not null)
        {
            var actionId = itemDB.ActionId == ActionTypeEnum.Added && actionTypeId != ActionTypeEnum.None
                ? ActionTypeEnum.Added
                : actionTypeId;

            var item = CloneShoppingCartItem(actionId, itemDB);

            item.Id = shoppingCartItem.Id;
            item.Quantity = shoppingCartItem.Quantity;

            await _database.UpdateAsync(item);
        }
    }

    public async Task<List<ShoppingCartItem>> GetShoppingCartItemsToSync(long? cartId)
    {
        await Init();

        return await _database.Table<ShoppingCartItem>()
            .Where(i => i.ActionId != ActionTypeEnum.None && i.CartId == cartId)
            .ToListAsync();
    }

    public async Task<List<ShoppingCartItem>> GetShoppingCartItems(long? cartId)
    {
        await Init();

        var items = await _database.Table<ShoppingCartItem>()
            .Where(i => i.CartId == cartId && i.ActionId != ActionTypeEnum.Deleted)
            .ToListAsync();
        return items;

    }

    public async Task<ShoppingCartItem?> GetLastShoppingCartItem()
    {
        await Init();

        var item = await _database.Table<ShoppingCartItem>()
            .OrderByDescending(i => i.ItemId)
            .FirstOrDefaultAsync();
        return item;
    }

    private static ShoppingCartItem CloneShoppingCartItem(ActionTypeEnum actionTypeId, ShoppingCartItem item)
    {
        ShoppingCartItem updatedItem = new ShoppingCartItem
        {
            Id = item.Id,
            ActionId = actionTypeId,
            CartId = item.CartId,
            ImageThumbnailUrl = item.ImageThumbnailUrl,
            ItemId = item.ItemId,
            Name = item.Name,
            PieId = item.PieId,
            Price = item.Price,
            Quantity = item.Quantity,
        };
        return updatedItem;
    }
}
