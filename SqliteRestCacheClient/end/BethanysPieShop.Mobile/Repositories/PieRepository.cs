using System.Net.Http.Json;
using System.Text;
using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.Repositories.Interfaces;
using System.Text.Json;

namespace BethanysPieShop.Mobile.Repositories;
public class PieRepository : Repository, IPieRepository
{
    private readonly string PiesFileName = "pies.json";
    private readonly string PiesOfWeekFileName = "piesofweek.json";
    private readonly string CacheDirectory = FileSystem.Current.CacheDirectory;

    public PieRepository(IHttpClientFactory httpClientFactory)
    : base(httpClientFactory)
    {
    }

    public async Task<PieDetailModel?> GetPieById(int id)
    {
        //NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (IsOnline)
        {

            HttpClient client = CreateHttpClient();

            PieDetailModel? pie = await client.GetFromJsonAsync<PieDetailModel>(
                $"api/pies/{id}",
                JsonSerializerOptions);

            return pie ?? new PieDetailModel();
        }

        var cachedPies = await GetCachedPies(PiesFileName);
        var pieModel = cachedPies.FirstOrDefault(p => p.Id == id);
        if (pieModel == null)
        {
            return null;
        }

        return new PieDetailModel
        {
            Id = pieModel.Id,
            CategoryId = pieModel.CategoryId,
            Name = pieModel.Name,
            Price = pieModel.Price,
            ImageThumbnailUrl = pieModel.ImageThumbnailUrl
        };
    }

    public async Task<List<PieModel>> GetPiesOfTheWeek()
    {
        var cachedPies = await GetCachedPies(PiesOfWeekFileName);

        if (cachedPies.Count > 0)
        {
            return cachedPies;
        }

        HttpClient client = CreateHttpClient();

        List<PieModel>? pieModels = await client.GetFromJsonAsync<List<PieModel>>(
            "api/pies/piesoftheweek",
            JsonSerializerOptions);

        await CachePies(PiesOfWeekFileName, pieModels);

        return pieModels ?? new List<PieModel>();
    }

    public async Task<List<PieModel>> GetAllPies(bool retrieveFromServer = false)
    {
        if (retrieveFromServer is not true)
        {
            var cachedPies = await GetCachedPies(PiesFileName);

            if (cachedPies.Count > 0)
            {
                return cachedPies;
            }
        }

        HttpClient client = CreateHttpClient();

        List<PieModel>? pieModels = await client.GetFromJsonAsync<List<PieModel>>(
            "api/pies",
            JsonSerializerOptions);

        await CachePies(PiesFileName, pieModels);

        return pieModels ?? new List<PieModel>();
    }

    private async Task<List<PieModel>> GetCachedPies(string filename)
    {
        string piesFile = Path.Combine(CacheDirectory, filename);
//        NetworkAccess accessType = Connectivity.Current.NetworkAccess;

        if (File.Exists(piesFile))
        {
            var creationTime = File.GetCreationTime(piesFile);
            // Don't delete the file if no internet available
            if (creationTime.AddHours(6) < DateTime.Now && IsOnline)
            {
                File.Delete(piesFile);
            }
            else
            {
                string content = await File.ReadAllTextAsync(piesFile);
                if (!string.IsNullOrEmpty(content))
                {
                    return JsonSerializer.Deserialize<List<PieModel>>(content) ?? new List<PieModel>();
                }
            }
        }

        return new List<PieModel>();
    }

    private async Task CachePies(string fileName, List<PieModel>? pieModels)
    {
        if (pieModels is not null && pieModels.Count > 0)
        {
            using (FileStream filestream = new FileStream(Path.Combine(FileSystem.CacheDirectory, fileName), FileMode.OpenOrCreate))
            {
                var data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(pieModels));
                await filestream.WriteAsync(data, 0, data.Length);
            }
        }
    }
}
