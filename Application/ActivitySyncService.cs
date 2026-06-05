public class ActivitySyncService
{
    private readonly ActivityApiService _apiService;
    private readonly IActivityRepository _repository;

    public ActivitySyncService(ActivityApiService apiService, IActivityRepository repository)
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
