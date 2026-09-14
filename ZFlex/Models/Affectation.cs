namespace ZFlex.Models
{
    public class Affectation
    {
        public Guid Id { get; set; }
        public CreatedBy? CreatedBy { get; set; }
        public DateOnly Date { get; set; }
        public Guid? DeskId { get; set; }
        public Guid? SpaceId { get; set; }
        public string? Moment { get; set; }
        public Guid? UserId { get; set; }
        public Guid? GuestId { get; set; }
        public object? Description { get; set; }
        public string? Type { get; set; }
        public bool Active { get; set; }
        public User? User { get; set; }
        public object? Guest { get; set; }
        public List<Service> Services { get; set; } = new();
        public string? __typename { get; set; }
    }
}
