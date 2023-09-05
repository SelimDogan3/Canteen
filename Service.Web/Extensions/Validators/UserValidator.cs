using Cantin.Entity.Entities;
using FluentValidation;

namespace Cantin.Service.Extensions.Validators
{
	public class UserValidator : AbstractValidator<AppUser>
	{
		public UserValidator() {
			RuleFor(x => x.FirstName)
				.NotEmpty()
				.WithName("İsim")
				;
			RuleFor(x => x.LastName)
				.NotEmpty()
				.WithName("Soyad")
				;
			RuleFor(x => x.PhoneNumber)
				.NotEmpty()
				.WithName("Telefon Numarası");
			RuleFor(x => x.Adress)
				.NotEmpty()
				.WithName("Adres")
				;
			RuleFor(x => x.Email)
				.NotEmpty()
				.WithName("mail")
				;

		}
	}
}
