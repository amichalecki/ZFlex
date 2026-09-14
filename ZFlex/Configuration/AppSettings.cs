using System.Text.Json;

namespace ZFlex.Configuration
{
    public class AppSettings
    {
        public int ReservationDayUntilNow { get; set; } = 1;
        public List<Guid> DeskAreas { get; set; } = new();
        public List<string> DeskPriority { get; set; } = new();
        public List<Guid> ParkingAreas { get; set; } = new();
        public List<string> ParkingPriority { get; set; } = new();
        public Guid UserId { get; set; }
        public required AuthSettings AuthSettings { get; set; }
        public required FlexClientSetup FlexClient { get; set; }
        public required string LogPath { get; set; }

        public static AppSettings Load(string path = "appsettings.json")
            => JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(path)!)!;
    }
}
