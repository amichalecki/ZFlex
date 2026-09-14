namespace ZFlex.Models.DeleteAffectations
{
    public class DeleteAffectationsVariables(IEnumerable<Guid> affectationsIds)
    {
        public List<Param> Params { get; set; } = affectationsIds
            .Select(id => new Param { AffectationId = id })
            .ToList();
    }

    public class Param
    {
        public Guid AffectationId { get; set; }
        public bool DeleteGuestsOf { get; set; } = false;
    }
}
