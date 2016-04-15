using System.Collections.Generic;
using GD.Models.Commons;

namespace GoldDataWeb.Models
{
	public class UploadFileViewModel
	{
		public string OrderNumber { get; set; }
		public string OrderId { get; set; }
		public string Comment { get; set; }
		public int OrderShotCount { get; set; }
		public int OrderShotType { get; set; }
		public string LinkType { get; set; }
		public string RadioBaseId { get; set; }
		public string Distance { get; set; }
		public string SiteId { get; set; }
	}
}
