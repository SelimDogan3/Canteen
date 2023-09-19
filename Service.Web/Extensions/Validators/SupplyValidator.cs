using Cantin.Entity.Entities;
using FluentValidation;

namespace Cantin.Service.Extensions.Validators
{
	public class SupplyValidator : AbstractValidator<Supply>
	{
		public SupplyValidator()
		{
			RuleFor(x => x.Supplier)
				.NotEmpty()
				.WithName("Tedarikçi")
				;
			RuleFor(x => x.UnitPrice)
				.NotNull()
				.GreaterThanOrEqualTo(0)
				.WithName("Ürün Birim Fiyatı")
				;
			RuleFor(x => x.Quantity)
				.NotEmpty()
				.NotNull()
				.GreaterThanOrEqualTo(1)
				.WithName("Ürün Miktarı")
				;
			RuleFor(x => x.ExpirationDate)
				.NotNull()
				.WithName("Son Tüketim Tarihi")
				;
		}
	}
}
