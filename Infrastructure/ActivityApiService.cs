using System.Net.Http.Json;
using TerreAventure.Domain.Interface;

public class ActivityApiService : IActivityApiService
{
    private readonly HttpClient _httpClient;
    public ActivityApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ActivityResponse?> FetchActivitiesAsync(string url)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ActivityResponse>(url);
        }
        catch (Exception ex)
        {
            // Log de l'erreur selon vos besoins
            Console.WriteLine($"Erreur lors de la récupération de l'API : {ex.Message}");
            return null;
        }
    }
}