using Cantin.Entity.Dtos.Products;
using Cantin.Entity.Dtos.Stores;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Cantin.Entity.Dtos.Supplies
{
    public class SupplyLine
    {
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
        public string stringCreatedDate => CreatedDate.ToString("dd.MM.yyyy HH:mm:ss", new CultureInfo("tr-TR"));
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float Total => Quantity * UnitPrice;
        public ProductDto Product { get; set; }
        public StoreDto Store { get; set; }
    }
}
