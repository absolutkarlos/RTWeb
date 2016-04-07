using System;
using Newtonsoft.Json;

namespace GD.Models.Commons
{
	public class OrderStatus
	{
		[JsonProperty(@"idorderstatus")]
		public int Id { get; set; }

		[JsonProperty(@"name")]
		public string Name { get; set; }

		[JsonProperty(@"color")]
		public string Color { get; set; }

		[JsonProperty(@"orderinflow")]
		public int? OrderInFlow { get; set; }

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
			Activo = 1,
			Inactivo = 2,
			Standby = 3,
			Aprobado = 4,
			Recazado = 5,
			Borrador = 6,
			Nuevo = 7,
			Prefactibilidad = 8,
			Inspeccion = 9,
			Instalacion = 10,
			Administracion = 11
		}
	}
}