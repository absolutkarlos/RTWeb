using System;
using Newtonsoft.Json;

namespace GD.Models.Commons
{
	public class Status
	{
		[JsonProperty(@"idstatus")]
		public int Id { get; set; }

		[JsonProperty(@"name")]
		public string Name { get; set; }

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
			Activo = 1,
			Inactivo = 2,
			Espera = 3
		}
	}
}