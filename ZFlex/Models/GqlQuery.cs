namespace ZFlex.Models
{
    public class GqlQuery<TVar>
    {
        public required string OperationName { get; set; }
        public required string Query { get; set; }
        public required TVar Variables { get; set; }
    }
}
