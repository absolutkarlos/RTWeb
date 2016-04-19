using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GD.Models.Commons.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GD.Models.Commons
{
	public class Material
	{
		[JsonProperty(@"idmaterial")]
		public int Id { get; set; }

		[JsonProperty(@"unitmeasure")]
		public UnitMeasure UnitMeasure { get; set; }

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


		[JsonProperty(@"idstatus")]
		public int IdStatus { get; set; }

		[JsonProperty(@"idunitmeasure")]
		public int IdUnitMeasure { get; set; }

		[JsonExtensionData]
		public readonly IDictionary<string, JToken> AdditionalData;


		public Material()
		{
			AdditionalData = new Dictionary<string, JToken>();
		}

		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			if ((UnitMeasure != null) && (UnitMeasure.Id.IsGreaterThanZero()))
			{
				IdUnitMeasure = UnitMeasure.Id;
			}

			if ((Status != null) && (Status.Id.IsGreaterThanZero()))
			{
				IdStatus = Status.Id;
			}
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (AdditionalData.ContainsKey(@"unitmeasure.idunitmeasure"))
			{
				UnitMeasure = new UnitMeasure
				{
					Id = (int)AdditionalData[@"unitmeasure.idunitmeasure"],
					Name = AdditionalData.ContainsKey(@"unitmeasure.name") ? AdditionalData[@"unitmeasure.name"].ToString() : string.Empty
				};
			}

			if (AdditionalData.ContainsKey(@"status.idstatus"))
			{
				Status = new Status
				{
					Id = (int)AdditionalData[@"status.idstatus"]
					//Name = AdditionalData[@"status.name"].ToString()
				};
			}

			AdditionalData.Clear();
		}

		public static string GetConcatMaterials(List<OrderMaterial> orderMaterials)
		{
			var concatMaterials = string.Empty;
			for (int i = 0; i < orderMaterials.Count; i++)
			{
				if (i > 0)
				{
					concatMaterials += ", ";
				}
				var element = orderMaterials.ElementAt(i);
				concatMaterials += element.Material.Name + @" (" + element.Quantity + @" " + element.Material.UnitMeasure.Name + @")";
			}
			return concatMaterials;
		}
	}
}