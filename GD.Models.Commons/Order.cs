using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GD.Models.Commons.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GD.Models.Commons
{
	public class Order
	{
		[JsonProperty(@"idorder")]
		public int Id { get; set; }

		[JsonProperty(@"site")]
		public Site Site { get; set; }

		[JsonProperty(@"orderstatus")]
		public OrderStatus OrderStatus { get; set; }

		[JsonProperty(@"installationdays")]
		public string InstallationDays { get; set; }

		[JsonProperty(@"ordernumber")]
		public string OrderNumber { get; set; }

		[JsonProperty(@"aditionalcost")]
		public string AditionalCost { get; set; }

		[JsonProperty(@"settingup")]
		public string SettingUp { get; set; }

		[JsonProperty(@"comments")]
		public string Comments { get; set; }

		[JsonProperty(@"specialrequirements")]
		public string SpecialRequirements { get; set; }

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


		[JsonProperty(@"listmaterials")]
		public IEnumerable<OrderMaterial> ListMaterials { get; set; }

		[JsonProperty(@"listordershot")]
		public IEnumerable<OrderShot> ListOrderShot { get; set; }

		[JsonProperty(@"listorderflow")]
		public IEnumerable<OrderFlow> ListOrderFlow { get; set; }


		[JsonProperty(@"idsite")]
		public int IdSite { get; set; }

		[JsonProperty(@"idorderstatus")]
		public int IdOrderStatus { get; set; }

		[JsonProperty(@"idstatus")]
		public int IdStatus { get; set; }



		[JsonExtensionData]
		public readonly IDictionary<string, JToken> AdditionalData;

		public Order()
		{
			AdditionalData = new Dictionary<string, JToken>();
		}

		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			if ((Site != null) && (Site.Id.IsGreaterThanZero()))
			{
				IdSite = Site.Id;
			}

			if ((OrderStatus != null) && (OrderStatus.Id.IsGreaterThanZero()))
			{
				IdOrderStatus = OrderStatus.Id;
			}

			if ((Status != null) && (Status.Id.IsGreaterThanZero()))
			{
				IdStatus = Status.Id;
			}
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (AdditionalData.ContainsKey(@"site.idsite"))
			{
				Site = new Site
				{
					Id = (int)AdditionalData[@"site.idsite"],
					Name = AdditionalData.ContainsKey(@"site.sitename") ? AdditionalData[@"site.sitename"].ToString() : string.Empty,
					BandWidth = AdditionalData.ContainsKey(@"site.bandwidth") ? (float)AdditionalData[@"site.bandwidth"] : 0,
					BuildingFloors = AdditionalData.ContainsKey(@"site.buildingfloors") ? (int)AdditionalData[@"site.buildingfloors"] : 0,
					Address = AdditionalData.ContainsKey(@"site.address") ? AdditionalData[@"site.address"].ToString() : string.Empty,
					FloorHight = AdditionalData.ContainsKey(@"site.floorhight") ? (float)AdditionalData[@"site.floorhight"] : 0,
					SiteNumber = AdditionalData.ContainsKey(@"site.sitenumber") ? (int)AdditionalData[@"site.sitenumber"] : 0,
					Latitude = AdditionalData.ContainsKey(@"site.latitude") ? AdditionalData[@"site.latitude"].ToString() : string.Empty,
					Longitude = AdditionalData.ContainsKey(@"site.longitude") ? AdditionalData[@"site.longitude"].ToString() : string.Empty,
					LinkType = AdditionalData.ContainsKey(@"site.linktype") ? AdditionalData[@"site.linktype"].ToString() : string.Empty,
					BuildingHight = AdditionalData.ContainsKey(@"site.buildinghight") ? (float)AdditionalData[@"site.buildinghight"] : 0
				};

				if (AdditionalData.ContainsKey(@"client.idclient"))
				{
					Site.Client = new Client
					{
						Id = (long)AdditionalData[@"client.idclient"],
						Ruc = AdditionalData.ContainsKey(@"client.ruc") ? AdditionalData[@"client.ruc"].ToString() : string.Empty,
						LegalName = AdditionalData.ContainsKey(@"client.legalname") ? AdditionalData[@"client.legalname"].ToString() : string.Empty,
						BusinessName = AdditionalData.ContainsKey(@"client.bussinessname") ? AdditionalData[@"client.bussinessname"].ToString() : string.Empty,
						AddressRef = AdditionalData.ContainsKey(@"client.addressref") ? AdditionalData[@"client.addressref"].ToString() : string.Empty,
						IdClientType = AdditionalData.ContainsKey(@"client.idclienttype") ? (int)AdditionalData[@"client.idclienttype"] : 0,
						SiteNumber = AdditionalData.ContainsKey(@"client.sitenumber") ? AdditionalData[@"client.sitenumber"].ToString() : string.Empty
					};
				}
			}

			if (AdditionalData.ContainsKey(@"orderstatus.idorderstatus"))
			{
				OrderStatus = new OrderStatus
				{
					Id = (int)AdditionalData[@"orderstatus.idorderstatus"],
					Name = AdditionalData.ContainsKey(@"orderstatus.name") ? AdditionalData[@"orderstatus.name"].ToString() : string.Empty,
					Color = AdditionalData.ContainsKey(@"orderstatus.color") ? AdditionalData[@"orderstatus.color"].ToString() : string.Empty,
					OrderInFlow = AdditionalData.ContainsKey(@"orderstatus.orderinflow") ? (string.IsNullOrWhiteSpace(AdditionalData[@"orderstatus.orderinflow"].ToString()) ? 0 : (int)AdditionalData[@"orderstatus.orderinflow"]) : 0
				};
			}

			if (AdditionalData.ContainsKey(@"status.idstatus"))
			{
				Status = new Status
				{
					Id = (int)AdditionalData[@"status.idstatus"],
					Name = AdditionalData.ContainsKey(@"status.name") ? AdditionalData[@"status.name"].ToString() : string.Empty
				};
			}

			AdditionalData.Clear();
		}
	}
}