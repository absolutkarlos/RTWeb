using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GD.Models.Commons.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GD.Models.Commons
{
	public class OrderFlow
	{
		[JsonProperty(@"idorderflow")]
		public int Id { get; set; }

		[JsonProperty(@"order")]
		public Order Order { get; set; }

		[JsonProperty(@"orderstatus")]
		public OrderStatus OrderStatus { get; set; }

		[JsonProperty(@"ordernextstatus")]
		public OrderStatus OrderNextStatus { get; set; }

		[JsonProperty(@"stimateddatenextstep")]
		public DateTime? StimatedDateNextStep { get; set; }

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



		[JsonProperty(@"idordernextstatus")]
		public int IdOrderNextStatus { get; set; }

		[JsonProperty(@"idorderstatus")]
		public int IdOrderStatus { get; set; }

		[JsonProperty(@"idorder")]
		public int IdOrder { get; set; }

		[JsonProperty(@"idstatus")]
		public int IdStatus { get; set; }




		[JsonExtensionData]
		public readonly IDictionary<string, JToken> AdditionalData;

		public OrderFlow()
		{
			AdditionalData = new Dictionary<string, JToken>();
		}

		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			if ((Order != null) && (Order.Id.IsGreaterThanZero()))
			{
				IdOrder = Order.Id;
			}

			if ((OrderStatus != null) && (OrderStatus.Id.IsGreaterThanZero()))
			{
				IdOrderStatus = OrderStatus.Id;
			}

			if ((OrderNextStatus != null) && (OrderNextStatus.Id.IsGreaterThanZero()))
			{
				IdOrderNextStatus = OrderNextStatus.Id;
			}

			if ((Status != null) && (Status.Id.IsGreaterThanZero()))
			{
				IdStatus = Status.Id;
			}
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (AdditionalData.ContainsKey(@"orderstatus.idorderstatus"))
			{
				OrderStatus = new OrderStatus
				{
					Id = (int)AdditionalData[@"orderstatus.idorderstatus"]
					//Name = AdditionalData[@"orderstatus.name"].ToString()
				};
			}

			if (AdditionalData.ContainsKey(@"ordernextstatus.idorderstatus"))
			{
				OrderStatus = new OrderStatus
				{
					Id = (int)AdditionalData[@"ordernextstatus.idorderstatus"]
					//Name = AdditionalData[@"orderstatus.name"].ToString()
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