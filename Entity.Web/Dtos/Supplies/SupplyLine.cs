﻿using Cantin.Core.Helpers;
using Cantin.Entity.Dtos.Products;
using Cantin.Entity.Dtos.Stores;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Cantin.Entity.Dtos.Supplies
{
    public class SupplyLine
    {
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
        public string StringCreatedDate => DateTimeFormatter.FormatForTr(CreatedDate);
        public DateTime ExpirationDate { get; set; }
        public string StringExpirationDate => ExpirationDate.ToString("dd.MM.yyyy", new CultureInfo("tr-TR"));
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float Total => Quantity * UnitPrice;
        public ProductDto Product { get; set; }
        public StoreDto Store { get; set; }
    }
}
