using System.Collections.Generic;

namespace GoldDataWeb.Models
{
	public class UploadFileViewModel
	{
		public string OrderNumber { get; set; }
		public string OrderId { get; set; }
		public string Comment { get; set; }
		public int OrderShotCount { get; set; }
		public int OrderShotType { get; set; }
	}
}
