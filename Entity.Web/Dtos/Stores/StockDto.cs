using Cantin.Entity.Entities;

namespace Cantin.Entity.Dtos.Stores
{
    public class StockDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public List<StockLine> Stocks { get; set; } = new List<StockLine>();
    }
}
