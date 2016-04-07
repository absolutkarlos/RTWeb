using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GD.Models.Commons.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GD.Models.Commons
{
	public class OrderShot
	{
		[JsonProperty(@"idordershot")]
		public int Id { get; set; }

		[JsonProperty(@"order")]
		public Order Order { get; set; }

		[JsonProperty(@"comment")]
		public string Comment { get; set; }

		[JsonProperty(@"shotpath")]
		public string ShotPath { get; set; }

		[JsonProperty(@"ordershottype")]
		public OrderShotType OrderShotType { get; set; }

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



		[JsonProperty(@"idordershottype")]
		public int IdOrderShotType { get; set; }

		[JsonProperty(@"idorder")]
		public int IdOrder { get; set; }

		[JsonProperty(@"idstatus")]
		public int IdStatus { get; set; }


		[JsonExtensionData]
		public readonly IDictionary<string, JToken> AdditionalData;

		public OrderShot()
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

			if ((OrderShotType != null) && (OrderShotType.Id.IsGreaterThanZero()))
			{
				IdOrderShotType = OrderShotType.Id;
			}

			if ((Status != null) && (Status.Id.IsGreaterThanZero()))
			{
				IdStatus = Status.Id;
			}
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (AdditionalData.ContainsKey(@"order.idorder"))
			{
				Order = new Order
				{
					Id = (int)AdditionalData[@"order.idorder"]
				};
			}

			if (AdditionalData.ContainsKey(@"ordershottype.idordershottype"))
			{
				OrderShotType = new OrderShotType
				{
					Id = (int) AdditionalData[@"ordershottype.idordershottype"],
					Name = AdditionalData.ContainsKey(@"ordershottype.name") ? AdditionalData[@"ordershottype.name"].ToString() : string.Empty
				};
			}

			if (AdditionalData.ContainsKey(@"status.idstatus"))
			{
				Status = new Status
				{
					Id = (int) AdditionalData[@"status.idstatus"]
					//Name = AdditionalData[@"status.name"].ToString()
				};
			}

			AdditionalData.Clear();
		}

		public enum Type
		{
			LineSight = 1,
			Instalation = 2
		}
	}
}