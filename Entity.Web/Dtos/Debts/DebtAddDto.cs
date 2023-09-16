using Cantin.Entity.Entities;

namespace Cantin.Entity.Dtos.Debts
{
    public class DebtAddDto
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string FullName => FirstName + " " + LastName;
        public List<ProductLine> ProductLines { get; set; }
        public float TotalPrice => (float)ProductLines.Sum(x => x.SubTotal);
        public bool Paid { get; set; } = false;
    }
}
