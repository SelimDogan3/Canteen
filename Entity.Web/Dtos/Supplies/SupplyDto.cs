namespace Cantin.Entity.Dtos.Supplies
{
    public class SupplyDto
    {
        public Guid Id { get; set; }
        public string Supplier { get; set; }
        public List<SupplyLine> SupplyLines { get; set; } = new List<SupplyLine>();
    }
}
