using System.Collections.Generic;
using GD.Models.Commons;

namespace GoldDataWeb.Models
{
	public class HomeViewModel
	{
		public IEnumerable<Order> Orders { get; set; }
		public User User { get; set; }
		public Order Order { get; set; }
	}
}