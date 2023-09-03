using Cantin.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;

namespace Cantin.Data.Identity
{
	public class AppIdentityUserValidator : IUserValidator<AppUser>
	{
		private readonly IdentityErrorDescriber Describer;

		public AppIdentityUserValidator(IdentityErrorDescriber describer)
		{
			Describer = describer;
		}
		public async Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager,AppUser user)
		{
			List<IdentityError> errors = new(); 
			await ValidateEmailAsync(manager,user,errors);
			return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
			}
		
		public async Task<List<IdentityError>> ValidateEmailAsync(UserManager<AppUser> manager,AppUser user, List<IdentityError> errors)
		{
			try
			{
				MailAddress adress = new MailAddress(user.Email);

			}
			catch (FormatException)
			{
				errors.Add(Describer.InvalidEmail(user.Email));
			}
			if (manager.Options.User.RequireUniqueEmail)
			{
				var duplicate = await manager.FindByEmailAsync(user.Email);
				if (duplicate != null)
				{
					errors.Add(Describer.DuplicateEmail(user.Email));
				}
			}
			return errors;
		}
	}
}

