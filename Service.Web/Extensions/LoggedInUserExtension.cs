using System.Security.Claims;

namespace Cantin.Service.Extensions
{
	public static class LoggedInUserExtension
	{
		public static string GetLoggedInUserEmail(this ClaimsPrincipal claimsPrincipal) {
			return claimsPrincipal.FindFirstValue(ClaimTypes.Email);
		}
		public static string GetLoggedInUserName(this ClaimsPrincipal principal) {
			return principal.FindFirstValue(ClaimTypes.NameIdentifier);
		}
		public static string GetRole(this ClaimsPrincipal principal)
		{
			return principal.FindFirstValue(ClaimTypes.Role);
		}
	}
}
