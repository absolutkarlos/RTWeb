using System;
using Newtonsoft.Json;

namespace GD.Models.Commons
{
	public class Rol
	{
		[JsonProperty(@"idrol")]
		public int Id { get; set; }

		[JsonProperty(@"name")]
		public string Name { get; set; }

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

		public enum Type
		{
			SysAdmin = 1,
			Ventas = 2,
			Admin = 3,
			Noc = 4,
			Field = 5,
			Manager = 6
		}
	}
}