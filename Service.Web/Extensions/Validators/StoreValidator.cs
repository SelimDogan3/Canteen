using Cantin.Entity.Entities;
using FluentValidation;

namespace Cantin.Service.Extensions.Validators
{
	public class StoreValidator : AbstractValidator<Store>
	{
        public StoreValidator()
        {
            RuleFor(x => x.Adress)
                .NotEmpty()
                .WithName("Adres")
                ;
            RuleFor(x => x.Name)
				.NotEmpty()
				.WithName("Mağaza Adı")
                ;
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithName("Telefon Numarası");

		}
    }
}
