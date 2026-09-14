namespace ZFlex.Models.DeleteAffectations
{
    public class DeleteAffectationsResponse
    {
        public Data Data { get; set; } = new();
    }

    public class Data
    {
        public DeleteAffectations DeleteAffectations { get; set; } = new();
    }

    public class DeleteAffectations
    {
        public List<Guid> DeletedServicesIds { get; set; } = new();
        public bool Success { get; set; } = false;
    }
}
