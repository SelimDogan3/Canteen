namespace Cantin.Entity.Dtos.Products
{
	public class ProductAddDto
	{
		public string Name { get; set; }
		public string Barcode { get; set; }
		public string SalePrice { get; set; }
        public bool MostUsed { get; set; } = false;
    }
}
