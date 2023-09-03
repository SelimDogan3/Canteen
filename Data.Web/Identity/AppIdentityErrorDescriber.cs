using Microsoft.AspNetCore.Identity;
namespace Cantin.Data.Identity
{
	public class AppIdentityErrorDescriber : IdentityErrorDescriber
	{
		public override IdentityError DuplicateEmail(string email)
		{
			ModelAddIdentityError error = new ModelAddIdentityError()
			{
				Code = nameof(DuplicateEmail),
				Description = $"{email} Adlı mail daha önce kayıt edilmiş",
				key = "Email",
			};
			return error;
		}
		public override IdentityError DuplicateUserName(string userName)
		{
			IdentityError error = new ModelAddIdentityError()
			{
				Code = nameof(DuplicateUserName),
				Description = $"{userName} Adlı mail daha önce kayıt edilmiş",
				key = "Email",
			};
			return error;
		}
		public override IdentityError InvalidEmail(string email)
		{
			ModelAddIdentityError error = new ModelAddIdentityError()
			{
				Code = nameof(InvalidEmail),
				Description = $"{email} mail adresi geçersizdir",
				key = "Email",
			};
			return error;
		}
		public override IdentityError InvalidUserName(string userName)
		{
			ModelAddIdentityError error = new ModelAddIdentityError()
			{
				Code = nameof(InvalidUserName),
				Description = $"{userName} kullanıcı adı geçersizdir",
				key = "Email",
			};
			return error;
		}
		public override IdentityError UserAlreadyInRole(string role)
		{
			ModelAddIdentityError error = new ModelAddIdentityError()
			{
				Code = nameof(UserAlreadyInRole),
				Description = $"{role} zaten kullanıcının rolleri arasında var",
				key = "RoleId",
			};
			return error;
		}
		public override IdentityError PasswordRequiresDigit()
		{
			ModelAddIdentityError error = new ModelAddIdentityError()
			{
				Code = nameof(PasswordRequiresDigit),
				Description = $"Parolanın içinde en az 1 tane sayi olmasi gerekmektedir",
				key = "password",
			};
			return error;
		}
		public override IdentityError PasswordRequiresLower()
		{
			ModelAddIdentityError error = new ModelAddIdentityError()
			{
				Code = nameof(PasswordRequiresLower),
				Description = $"Parolanın içinde en az 1 tane küçük harf olmasi gerekmektedir",
				key = "password",


			};
			return error;
		}
		public override IdentityError PasswordRequiresUpper()
		{
			ModelAddIdentityError error = new ModelAddIdentityError()
			{
				Code = nameof(PasswordRequiresUpper),
				Description = $"Parolanın içinde en az 1 tane büyük harf olmasi gerekmektedir",
				key = "password",

			};
			return error;
		}
		public override IdentityError PasswordTooShort(int length)
		{
			ModelAddIdentityError error = new ModelAddIdentityError()
			{
				Code = nameof(PasswordTooShort),
				Description = $"Parolanız gerekli miktarda uzun değildir en az {length}",
				key = "password",

			};
			return error;
		}
		public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
		{
			ModelAddIdentityError error = new ModelAddIdentityError()
			{
				Code = nameof(PasswordRequiresUniqueChars),
				Description = $"Parolanızda yeterli miktarda özel karakter yoktur en az {uniqueChars}",
				key = "password",


			};
			return error;
		}
		public override IdentityError PasswordRequiresNonAlphanumeric()
		{
			ModelAddIdentityError error = new ModelAddIdentityError()
			{
				Code = nameof(PasswordRequiresNonAlphanumeric),
				Description = $"Parolanızda yanlızca sayısal değerler kullanabilirsiniz",
				key = "password",


			};
			return error;
		}
		public override IdentityError PasswordMismatch()
		{
			ModelAddIdentityError error = new ModelAddIdentityError()
			{
				Code = nameof(PasswordMismatch),
				Description = $"Kullanıcı ile parola uyuşmadı",
				key = "password",

			};
			return error;
		}
		public override IdentityError UserAlreadyHasPassword()
		{
			ModelAddIdentityError error = new ModelAddIdentityError()
			{
				Code = nameof(UserAlreadyHasPassword),
				Description = $"Kullanıcının zaten bir parolası var",
				key = "password",

			};
			return error;
		}
		public override IdentityError DuplicateRoleName(string role)
		{
			ModelAddIdentityError error = new ModelAddIdentityError()
			{
				Code = nameof(DuplicateRoleName),
				Description = $"{role} adlı rol zaten mevcut",
				key = "Name",
			};
			return error;
		}
		public override IdentityError InvalidRoleName(string role)
		{
			ModelAddIdentityError error = new ModelAddIdentityError()
			{
				Code = nameof(DuplicateRoleName),
				Description = $"{role} rol adı geçersizdir",
				key = "Name",

			};
			return error;
		}
		public override IdentityError UserNotInRole(string role)
		{
			ModelAddIdentityError error = new ModelAddIdentityError()
			{
				Code = nameof(DuplicateRoleName),
				Description = $"bu kullanıcıda {role} rolü yok",
				key = "RoleId",
			};
			return error;
		}
	}
}
