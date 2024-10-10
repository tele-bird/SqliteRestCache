using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.Repositories.Interfaces;

namespace BethanysPieShop.Mobile.Repositories;
public class CategoryRepository : Repository, ICategoryRepository
{
    private readonly string CategoriesFileName = "categories.json";
    private readonly string CacheDirectory = FileSystem.Current.CacheDirectory;

    public CategoryRepository(IHttpClientFactory httpClientFactory) 
        : base(httpClientFactory)
    {
    }

    public async Task<List<CategoryModel>> GetAllCategories(bool retrieveFromServer = false)
    {
        string categoriesFile = Path.Combine(CacheDirectory, CategoriesFileName);

        //NetworkAccess accessType = Connectivity.Current.NetworkAccess;

        if (retrieveFromServer is not true)
        {
            if (File.Exists(categoriesFile))
            {
                var creationTime = File.GetCreationTime(categoriesFile);
                if (creationTime.AddHours(6) < DateTime.Now && IsOnline)
                {
                    File.Delete(categoriesFile);
                }
                else
                {
                    string content = await File.ReadAllTextAsync(categoriesFile);
                    if (!string.IsNullOrEmpty(content))
                    {
                        return JsonSerializer.Deserialize<List<CategoryModel>>(content) ?? new List<CategoryModel>();
                    }
                }
            }
        }

        HttpClient client = CreateHttpClient();

        List<CategoryModel>? categoryModels = await client.GetFromJsonAsync<List<CategoryModel>>(
            "api/categories",
            JsonSerializerOptions);

        var categoriesContent = JsonSerializer.Serialize(categoryModels);
        await File.WriteAllTextAsync(categoriesFile, categoriesContent);

        return categoryModels ?? new List<CategoryModel>();
    }
}
