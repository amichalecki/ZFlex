namespace ZFlex.Models.MapView
{
    public class MapViewVariables(Guid spaceId, Guid userId, DateOnly date)
    {
        public bool SkipOfficeEquipment { get; set; } = true;
        public bool SkipAffectations { get; set; } = false;
        public Guid SpaceId { get; set; } = spaceId;
        public AffectationData AffectationData { get; set; } = new() 
        { 
            Date = date 
        };
        public UserId MainUserIdV2 { get; set; } = new() 
        { 
            Id = userId 
        };
        public SpaceAvailibilityData SpaceAvailibilityData { get; set; } = new()
        {
            UserIds = [new() { Id = userId }],
            MainUserId = userId,
            Date = date
        };
        public DateOnly Date { get; set; } = date;
    }

    public class AffectationData
    {
        public DateOnly Date { get; set; }
        public bool AllDay { get; set; } = true;
        public string Moment { get; set; } = "MORNING";
    }

    public class SpaceAvailibilityData
    {
        public bool IsRecurrence { get; set; } = false;
        public int NbOfGuests { get; set; } = 0;
        public List<UserId> UserIds { get; set; } = new();
        public Guid MainUserId { get; set; }
        public DateOnly Date { get; set; }
    }
}
