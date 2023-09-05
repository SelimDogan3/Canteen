using Cantin.Entity.Entities;
using FluentValidation;

namespace Cantin.Service.Extensions.Validators
{
	public class ProductValidator : AbstractValidator<Product>
	{
        public ProductValidator()
        {
			RuleFor(x => x.Name)
				.NotEmpty()
				.WithName("Ürün İsmi")
				;
			RuleFor(x => x.Barcode)
				.NotEmpty()
				.WithName("Ürün Barkodu")
				;
			RuleFor(x => x.SalePrice)
				.NotEmpty()
				.WithName("Ürün Satış Fiyatı");
		}
    }
}
