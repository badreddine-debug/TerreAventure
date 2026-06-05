using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Configuration de la chaîne de connexion
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                          ?? "Server=YOUR_SERVER;Database=YOUR_DB;Trusted_Connection=True;TrustServerCertificate=True;";

// Injection des dépendances
builder.Services.AddHttpClient<ActivityApiService>();
builder.Services.AddScoped<IActivityRepository>(provider => new ActivityRepository(connectionString));
builder.Services.AddScoped<ActivitySyncService>();

var host = builder.Build();

// Exemple d'exécution directe pour le test
using (var scope = host.Services.CreateScope())
{
    var syncService = scope.ServiceProvider.GetRequiredService<ActivitySyncService>();
    string apiUrl = "https://api.exemple.com/users?page=2&size=100";

    // Remplacer par un vrai appel, ici entouré d'un try/catch car l'URL exemple va échouer
    try
    {
        await syncService.SyncActivitiesAsync(apiUrl);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exécution terminée (Vérifiez l'URL de votre API) : {ex.Message}");
    }
}