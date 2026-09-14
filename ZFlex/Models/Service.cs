namespace ZFlex.Models
{
    public class Service
    {
        public Guid Id { get; set; }
        public string? Type { get; set; }
        public Guid? ParkingSpotId { get; set; }
        public string? __typename { get; set; }
    }
}
