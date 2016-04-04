using System;
using System.Net;

namespace GoldDataWeb.Models
{
	public class ResponseService<TModel>
	{
		public TModel Data { get; set; }
		public HttpStatusCode Status { get; set; }
		public string ErrorMessage { get; set; }
	}
}
