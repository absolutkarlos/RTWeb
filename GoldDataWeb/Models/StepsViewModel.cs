using System.Collections.Generic;
using GD.Models.Commons;

namespace GoldDataWeb.Models
{
	public class StepsViewModel
	{
		public Order Order { get; set; }
		public Client Client { get; set; }
		public EntityContact EntityContact { get; set; }
		public Site Site { get; set; }
		public string CountryAbbrevation { get; set; }
		public LineSight LineSight { get; set; }
		public OrderFlow OrderFlow { get; set; }
		public bool IsUpdate { get; set; }
	}
}