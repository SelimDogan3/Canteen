using Microsoft.AspNetCore.Identity;

namespace Cantin.Data.Identity
{
	public class ModelAddIdentityError : IdentityError
	{
		public string key { get; set; }
	}
}
