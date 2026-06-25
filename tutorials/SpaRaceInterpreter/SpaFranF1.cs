//PROJECT TO HELP UNDERSTAND API USAGE!
using System.Net.Http.Json; //HELP C# download and read JSON
using System.Text.Json.Serialization; // helps C# match JSON field to MY CLASS PROPERTY

Console.WriteLine ("=== Spa-Francorchamps 2023 Race Interpreter ===");
Console.WriteLine();

HttpClient client = new HttpClient(); //Create a TOOL for C# to talk to the API!
string sessionUrl = "https://api.openf1.org/v1/sessions?country_name=Belgium&session_name=Race&year=2023";

//C# goes to the API, gets the JSON, and turns it into a LIST of SESSION objects!
List<Session>? sessions = await client.GetFromJsonAsync<List<Session>>(sessionUrl);

if ( sessions == null || sessions.Count == 0 ) {
    Console.WriteLine("No sessions found for the specified criteria.");
    return;
}

Session spaRace = sessions[0]; // Assuming the first session is the one we want

Console.WriteLine("Race Session Found, Details:");
Console.WriteLine($"Session Key: {spaRace.SessionKey}");
Console.WriteLine($"Meeting Key: {spaRace.MeetingKey}");
Console.WriteLine($"Country: {spaRace.Country}");
Console.WriteLine($"Track: {spaRace.CircuitShortName}");
Console.WriteLine($"Location: {spaRace.Location}");
Console.WriteLine($"Session: {spaRace.SessionName}");

string driverUrl = $"https://api.openf1.org/v1/drivers?session_key={spaRace.SessionKey}";
await Task.Delay(1000);
string resultsUrl = $"https://api.openf1.org/v1/session_result?session_key={spaRace.SessionKey}";
await Task.Delay(1000); //helps the API not get called to many time at once
string lapsUrl = $"https://api.openf1.org/v1/laps?session_key={spaRace.SessionKey}";

List<Driver>? drivers = await client.GetFromJsonAsync<List<Driver>>(driverUrl);
List<SessionResult>? results = await client.GetFromJsonAsync<List<SessionResult>>(resultsUrl);
List<Lap>? laps = await client.GetFromJsonAsync<List<Lap>>(lapsUrl);

if (drivers == null || results == null || laps == null) {
    Console.WriteLine("No drivers or results found for the specified session.");
    return;
}

Console.WriteLine();
Console.WriteLine("=== Final Race Results ===");

//Take the result LIST -> REMOVE null position -> SORT by position -> PUT into sortedResults
List<SessionResult> sortedResults = results.Where(result => result.Position != null).OrderBy(result => result.Position).ToList();

// Go through every race results in sortResults
foreach (SessionResult result in sortedResults)
{
    //For each driver d, check if d.DriverNumber == result.DriverNumber, if so, return that driver object, else return null
    Driver? driver = drivers.FirstOrDefault(d => d.DriverNumber == result.DriverNumber);

    //This is an subtitute for an IF statement
    //If the driver is null, say "Unknown Driver" is not null, show name
    string driverName = driver == null ? "Unknown Driver" : driver.FullName;

    //If the driver is null -> "Unknown Team"
    string teamName = driver == null ? "Unknown Team" : driver.TeamName;

    Console.WriteLine($"{result.Position}. {driverName} - {teamName}");
}

Console.WriteLine();
Console.WriteLine("=== Basic Race Interpreter ===");

SessionResult? winner = sortedResults.FirstOrDefault(); //get the first result -> THATS THE WINNER

if (winner != null) //becuase FirstOfDauflt can return NULL so to make sure NO NULL
{
    //serach drivers list!
    //find the first driver d where d.DriverNumber = winner.DriverNumber
    Driver? winnerDriver = drivers.FirstOrDefault(d => d.DriverNumber == winner.DriverNumber);
    
    if (winnerDriver != null)
    {
        Console.WriteLine($"{winnerDriver.FullName} won the 2023 Belgian Grand Prix for {winnerDriver.TeamName}.");
    }
}

Console.WriteLine();
Console.WriteLine("=== Fastets Lap Analysis ==="); //annother sorting list

//FirstOrDefault returns 1 lap -> not List<Lap>
Lap? fastestLap = laps.Where(lap => lap.LapDuration != null).OrderBy( lap => lap.LapDuration).FirstOrDefault();

if (fastestLap != null)
{
    Driver? fastestDriver = drivers.FirstOrDefault(d => d.DriverNumber == fastestLap.DriverNumber);

    if (fastestDriver != null)
    {
        Console.WriteLine($"{fastestDriver.FullName} had the fastest lap.");
        Console.WriteLine($"Lap Number: {fastestLap.LapNumber}");
        Console.WriteLine($"Lap Time: {fastestLap.LapDuration}");
    }
}

Console.WriteLine();
Console.WriteLine("=== Average Lap Pace ===");

//When u declare a variable with VAR -> the type is based on the VALUE assigned!
var averageLapTime = laps
    .Where(lap => lap.LapDuration != null) // Removes laps that do not have lap time
    .GroupBy(lap => lap.DriverNumber) //Groups lap by driver
    .Select(group => new
    {
        DriverNumber = group.Key,
        AverageLapTime = group.Average(lap => lap.LapDuration!.Value) //! is to check it is not null
    })
    .OrderBy(driver => driver.AverageLapTime)
    .ToList();

foreach (var driverAverage in averageLapTime)
{
    Driver? driver = drivers.FirstOrDefault(d => d.DriverNumber == driverAverage.DriverNumber);

    if (driver != null)
    {
        Console.WriteLine($"{driver.FullName} - {driverAverage.AverageLapTime:F3} seconds average");
    }
}

public class Session
{
    [JsonPropertyName("session_key")] //Fetch session key from the JSON
    public int SessionKey { get; set; } //PUT it in the SESSION object

    [JsonPropertyName("meeting_key")]
    public int MeetingKey { get; set; }

    [JsonPropertyName("country_name")]
    public string Country { get; set; } = "";

    [JsonPropertyName("circuit_short_name")]
    public string CircuitShortName { get; set; } = "";

    [JsonPropertyName("location")]
    public string Location { get; set; } = "";

    [JsonPropertyName("session_name")]
    public string SessionName { get; set; } = "";
}

public class Driver
{
    [JsonPropertyName("driver_number")]
    public int DriverNumber { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = "";

    [JsonPropertyName("team_name")]
    public string TeamName { get; set; } = "";

    [JsonPropertyName("name_acronym")]
    public string NameAcronym { get; set; } = "";
}

public class SessionResult
{
    [JsonPropertyName("driver_number")]
    public int DriverNumber { get; set; }
    
    [JsonPropertyName("position")]
    public int? Position { get; set; }

    [JsonPropertyName("number_of_laps")]
    public int? NumberOfLaps { get; set; }

    [JsonPropertyName("dnf")]
    public bool? Dnf { get; set; }

    [JsonPropertyName("dns")]
    public bool? Dns { get; set; }

    [JsonPropertyName("dsq")]
    public bool? Dsq { get; set; }
}

public class Lap
{
    [JsonPropertyName("lap_duration")]
    public double? LapDuration { get; set; }

    [JsonPropertyName("lap_number")]
    public int LapNumber { get; set; }

    [JsonPropertyName("driver_number")]
    public int DriverNumber { get; set; }
}