namespace ZFlex.Models.AffectationsByUserAndDates
{
    public class AffectationsByUserAndDatesVariables(Guid userId, IEnumerable<DateOnly> dates)
    {
        public UserId UserId { get; set; } = new() 
        { 
            Id = userId 
        };
        public AffectationsFilter AffectationsFilter { get; set; } = new() 
        { 
            Dates = dates.ToList()
        };
    }

    public class AffectationsFilter
    {
        public List<DateOnly> Dates { get; set; } = [DateOnly.FromDateTime(DateTime.Now.AddDays(14))];
        public bool withAuthoredSuggestions { get; set; } = true;
    }
}
