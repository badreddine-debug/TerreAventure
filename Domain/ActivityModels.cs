using System.Collections.Generic;
using System.Text.Json.Serialization;

public class ActivityResponse
{
    [JsonPropertyName("activities")]
    public List<ActivityDto> Activities { get; set; } = new();
}

public class ActivityDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; } // Deviendra IdActivateJson en BD

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("location")]
    public string Location { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("duration")]
    public string Duration { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; }
}

// Domain/Interfaces/IActivityRepository.cs
public interface IActivityRepository
{
    Task UpsertActivitiesAsync(IEnumerable<ActivityDto> activities);
}
