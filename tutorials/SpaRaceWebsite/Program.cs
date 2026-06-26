using System.Net.Http.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/location", async () =>
{
    HttpClient client = new HttpClient();
    int spaRaceSessionKey = 9141;
    int driverNumber = 1;

    string locationUrl = $"https://api.openf1.org/v1/location?session_key={spaRaceSessionKey}&driver_number={driverNumber}";
    //string pitUrl = $
    List<LocationPoint>? locations = await client.GetFromJsonAsync<List<LocationPoint>>(locationUrl);

    if (locations == null || locations.Count == 0)
    {
        return Results.NotFound("No location data found.");
    }

    var usableLocations = locations
    .Where(point => point.X != 0 || point.Y != 0)
    .Take(2000)
    .ToList();

    return Results.Ok(usableLocations);
});

Console.WriteLine ("=== Spa-Francorchamps 2023 Race ===");
Console.WriteLine();

app.Run();

public class LocationPoint
{
    [JsonPropertyName("date")]
    public string Date { get; set; } = "";

    [JsonPropertyName("driver_number")]
    public int DriverNumber { get; set; }

    [JsonPropertyName("session_key")]
    public int SessionKey { get; set; }

    [JsonPropertyName("x")]
    public double X { get; set; }

    [JsonPropertyName("y")]
    public double Y { get; set; }

    [JsonPropertyName("z")]
    public double Z { get; set; }
}
