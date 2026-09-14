namespace ZFlex.Models.MapView
{
    public class MapViewResponse
    {
        public required Data Data { get; set; }
    }

    public class Data
    {
        public Space? Space { get; set; }
    }

    public class ParkingSpot
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool IsLocked { get; set; }
        public bool IsLockedAt { get; set; }
        public Guid SpaceId { get; set; }
        public List<object> ExclusiveUserParkingSpotAffectations { get; set; } = new();
        public List<object> Availability { get; set; } = new();
        public string? __typename { get; set; }
    }

    public class Space
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public List<SpaceLeaf> SpaceLeaves { get; set; } = new();
        public string? __typename { get; set; }
    }

    public class SpaceAvailibility
    {
        public bool IsAvailable { get; set; }
        public int BookablePlaces { get; set; }
        public string? FailureMessage { get; set; }
        public string? __typename { get; set; }
    }

    public class SpaceLeaf
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? InheritedName { get; set; }
        public string? ServiceType { get; set; }
        public string? SpaceType { get; set; }
        public bool IsCommonSpace { get; set; }
        public int AffectationCount { get; set; }
        public int RealCapacity { get; set; }
        public List<Desk> Desks { get; set; } = new();
        public List<ParkingSpot> ParkingSpots { get; set; } = new();
        public List<Affectation> Affectations { get; set; } = new();
        public SpaceAvailibility? SpaceAvailibility { get; set; }
        public string? __typename { get; set; }
    }

    public class Desk
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool IsLocked { get; set; }
        public bool IsLockedAt { get; set; }
        public Guid SpaceId { get; set; }
        public object? ExclusiveUser { get; set; }
        public List<object> ExclusiveUserAffectationMoment { get; set; } = new();
        public string? __typename { get; set; }
    }
}
