namespace Cantin.Entity.Dtos.Products
{
	public class ProductUpdateDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Barcode { get; set; }
		public string SalePrice { get; set; }

	}
}
