using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GD.Models.Commons.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GD.Models.Commons
{
	public class OrderMaterial
	{
		[JsonProperty(@"idordermaterial")]
		public int Id { get; set; }

		[JsonProperty(@"material")]
		public Material Material { get; set; }

		[JsonProperty(@"quantity")]
		public float Quantity { get; set; }

		[JsonProperty(@"order")]
		public Order Order { get; set; }

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


		[JsonProperty(@"idmaterial")]
		public int IdMaterial { get; set; }

		[JsonProperty(@"idorder")]
		public int IdOrder { get; set; }

		[JsonProperty(@"idstatus")]
		public int IdStatus { get; set; }


		[JsonExtensionData]
		public readonly IDictionary<string, JToken> AdditionalData;

		public OrderMaterial()
		{
			AdditionalData = new Dictionary<string, JToken>();
		}

		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			if ((Material != null) && (Material.Id.IsGreaterThanZero()))
			{
				IdMaterial = Material.Id;
			}

			if ((Order != null) && (Order.Id.IsGreaterThanZero()))
			{
				IdOrder = Order.Id;
			}

			if ((Status != null) && (Status.Id.IsGreaterThanZero()))
			{
				IdStatus = Status.Id;
			}
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (AdditionalData.ContainsKey(@"material.idmaterial"))
			{
				Material = new Material
				{
					Id = (int)AdditionalData[@"material.idmaterial"]
					//Name = AdditionalData[@"material.name"].ToString()
				};
			}

			if (AdditionalData.ContainsKey(@"order.idorder"))
			{
				Order = new Order
				{
					Id = (int)AdditionalData[@"order.idorder"]
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
	}
}