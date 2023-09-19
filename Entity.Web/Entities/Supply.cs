using Cantin.Core.Entities;

namespace Cantin.Entity.Entities
{
	public class Supply : EntityBase
	{
		public string Supplier { get; set; }
		public Guid ProductId { get; set; }
		public Product? Product { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Quantity { get; set; }
		public float UnitPrice { get; set; }
		public Guid StoreId { get; set; }
		public Store? Store { get; set; }
	}
}
