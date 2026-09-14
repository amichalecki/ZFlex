namespace ZFlex.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PictureUrl { get; set; }
        public List<Affectation> Affectations { get; set; } = new();
        public FirstEditableDayMomentOfCaller? FirstEditableDayMomentOfCaller { get; set; }
        public string? __typename { get; set; }
    }
}
