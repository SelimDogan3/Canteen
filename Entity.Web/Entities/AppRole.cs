using Microsoft.AspNetCore.Identity;

namespace Cantin.Entity.Entities
{
	public class AppRole : IdentityRole<Guid>
    {
		public string Description { get; set; } = string.Empty;

	}
}
