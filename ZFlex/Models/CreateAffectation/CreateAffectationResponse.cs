namespace ZFlex.Models.CreateAffectation
{
    public class CreateAffectationResponse
    {
        public CreateAffectations? Data { get; set; }
    }

    public class CreateAffectations
    {
        public List<CreateAffectationEntry> CreateAffectation { get; set; } = new();
    }

    public class CreateAffectationEntry
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public object? GuestId { get; set; }
        public Guid DeskId { get; set; }
        public Guid SpaceId { get; set; }
        public List<Models.Service> Services { get; set; } = new();
        public string? __typename { get; set; }
    }
}
