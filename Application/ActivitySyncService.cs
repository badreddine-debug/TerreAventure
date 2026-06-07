using TerreAventure.Domain.Interface;
using TerreAventure.Domain.IRepository;

public class ActivitySyncService
{
    private readonly IActivityApiService _apiService; // Changé ici
    private readonly IActivityRepository _repository;

    public ActivitySyncService(IActivityApiService apiService, IActivityRepository repository)
    {
        _apiService = apiService;
        _repository = repository;
    }

    public async Task SyncActivitiesAsync(string url)
    {
        // 1. Extraction du JSON depuis l'URL
        var data = await _apiService.FetchActivitiesAsync(url);

        if (data != null && data.Activities.Any())
        {
            // 2. Sauvegarde et mise à jour (Upsert via MERGE)
            await _repository.UpsertActivitiesAsync(data.Activities);
            Console.WriteLine($"{data.Activities.Count} activités synchronisées avec succès.");
        }
    }
}
