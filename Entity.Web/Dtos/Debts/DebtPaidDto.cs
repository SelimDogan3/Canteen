namespace Cantin.Entity.Dtos.Debts
{
    public class DebtPaidDto
    {
        public Guid Id { get; set; }
        public float Exchange { get; set; }
        public float PaidAmount { get; set; }
        public string PaymentType { get; set; }
        public DateTime PaidDate => DateTime.Now;
        public bool Paid { get; set; } = true;
    }
}
