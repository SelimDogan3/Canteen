using Cantin.Core.Entities;

namespace Cantin.Entity.Entities
{
    public class Product : EntityBase
    {
        public string Name{ get; set; }
        public string Barcode { get; set; }
        public string SalePrice { get; set; }
        public bool MostUsed { get; set; } = false;
        public ICollection<Supply> Stocks { get; set; }
        public ICollection<Stock> StoreProducts { get; set; }
		public ICollection<SaleProduct> SaleProducts { get; set; }
		public ICollection<DebtProduct> DebtProducts { get; set; }
        public ICollection<ManuelStockReduction> ManuelStockReductions { get; set; }

    }
}
