using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantin.Entity.Dtos.Stores
{
	public class StoreDto
	{
        public Guid Id { get; set; }
        public string Name { get; set; }
		public string Adress { get; set; }
		public string PhoneNumber { get; set; }

	}
}
