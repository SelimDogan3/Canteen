using Cantin.Entity.Entities;
using FluentValidation;

namespace Cantin.Service.Extensions.Validators
{
	public class RoleValidator : AbstractValidator<AppRole>
	{
		public RoleValidator() {
			RuleFor(x => x.Name)
					.NotEmpty()
					.WithName("Rol Adı")
					;
			RuleFor(x => x.Description)
				.NotEmpty()
				.WithName("Rol Açıklaması")
				;
		}

	}
}
