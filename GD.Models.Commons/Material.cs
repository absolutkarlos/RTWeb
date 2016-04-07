using System;
using Newtonsoft.Json;

namespace GD.Models.Commons
{
	public class Material
	{
		[JsonProperty(@"idmaterial")]
		public int Id { get; set; }

		[JsonProperty(@"idunitmeasure")]
		public int IdUnitMeasure { get; set; }

		[JsonProperty(@"name")]
		public string Name { get; set; }

		[JsonProperty(@"quantity")]
		public int Quantity { get; set; }

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