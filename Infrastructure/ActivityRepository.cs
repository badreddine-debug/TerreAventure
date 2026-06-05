using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

public class ActivityRepository : IActivityRepository
{
    private readonly string _connectionString;

    public ActivityRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task UpsertActivitiesAsync(IEnumerable<ActivityDto> activities)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        db.Open();

        using var transaction = db.BeginTransaction();

        try
        {
            // 1. Création de la table temporaire locale
            await db.ExecuteAsync(@"
                CREATE TABLE #TempActivities (
                    Id INT,
                    Title NVARCHAR(200),
                    Location NVARCHAR(200),
                    Price DECIMAL(10,2),
                    Duration NVARCHAR(50),
                    Category NVARCHAR(50)
                );", transaction: transaction);

            // 2. Insertion des données reçues dans la table temporaire
            var insertQuery = @"
                INSERT INTO #TempActivities (Id, Title, Location, Price, Duration, Category)
                VALUES (@Id, @Title, @Location, @Price, @Duration, @Category);";

            await db.ExecuteAsync(insertQuery, activities, transaction: transaction);

            // 3. Exécution du MERGE (Insert ou Update)
            var mergeQuery = @"
                MERGE Activities AS Target
                USING #TempActivities AS Source
                ON (Target.IdActivateJson = Source.Id)
                
                -- Si correspondance : On met à jour
                WHEN MATCHED THEN
                    UPDATE SET 
                        Target.Title = Source.Title,
                        Target.Location = Source.Location,
                        Target.Price = Source.Price,
                        Target.Duration = Source.Duration,
                        Target.Category = Source.Category
                
                -- Si pas de correspondance : On insère
                WHEN NOT MATCHED THEN
                    INSERT (IdActivateJson, Title, Location, Price, Duration, Category)
                    VALUES (Source.IdActivateJson, Source.Title, Source.Location, Source.Price, Source.Duration, Source.Category);";

            await db.ExecuteAsync(mergeQuery, transaction: transaction);

            // Validation de la transaction
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}