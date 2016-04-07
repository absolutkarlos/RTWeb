using System;
using Newtonsoft.Json;

namespace GD.Models.Commons
{
	public class Audit
	{
		[JsonProperty(@"idAudit")]
		public int Id { get; set; }

		[JsonProperty(@"idobject")]
		public long IdObject { get; set; }

		[JsonProperty(@"object")]
		public string Object { get; set; }

		[JsonProperty(@"actionType")]
		public string ActionType { get; set; }

		[JsonProperty(@"comment")]
		public string Comment { get; set; }

		[JsonProperty(@"status")]
		public Status Status { get; set; }

		[JsonProperty(@"createdby")]
		public short? CreateBy { get; set; }

		[JsonProperty(@"createdat")]
		public DateTime? CreateAt { get; set; }

		[JsonProperty(@"updatedby")]
		public short? UpdateBy { get; set; }

		[JsonProperty(@"updatedat")]
		public DateTime? UpdateAt { get; set; }
	}
}