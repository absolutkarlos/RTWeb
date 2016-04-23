using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using GD.Models.Commons.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GD.Models.Commons
{
	public class Site
	{
		[JsonProperty(@"idsite")]
		public int Id { get; set; }

		[JsonProperty(@"linktype")]
		public string LinktType { get; set; }

		[JsonProperty(@"client")]
		public Client Client { get; set; }

		[JsonProperty(@"bandwidth")]
		public float BandWidth { get; set; }

		[JsonProperty(@"longitude")]
		public string Longitude { get; set; }

		[JsonProperty(@"latitude")]
		public string Latitude { get; set; }

		[JsonProperty(@"address")]
		public string Address { get; set; }

		[JsonProperty(@"siteNumber")]
		public int SiteNumber { get; set; }

		[JsonProperty(@"floorhight")]
		public float FloorHight { get; set; }

		[JsonProperty(@"buildinghight")]
		public float BuildingHight { get; set; }

		[JsonProperty(@"buildingfloors")]
		public int BuildingFloors { get; set; }

		[JsonProperty(@"sitename")]
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


		public IEnumerable<LineSight> ListLineSight { get; set; }
		public IEnumerable<ServiceType> ListServiceType { get; set; }
		public IEnumerable<SiteSchedule> ListSiteSchedule { get; set; }
		public IEnumerable<SiteAccessType> ListSiteAccessType { get; set; }


		[JsonProperty(@"idclient")]
		public int IdClient { get; set; }

		[JsonProperty(@"idstatus")]
		public int IdStatus { get; set; }


		[JsonExtensionData]
		public readonly IDictionary<string, JToken> AdditionalData;

		public Site()
		{
			AdditionalData = new Dictionary<string, JToken>();
		}

		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			if ((Client != null) && (Client.Id.IsGreaterThanZero()))
			{
				IdClient = (int)Client.Id;
			}

			if ((Status != null) && (Status.Id.IsGreaterThanZero()))
			{
				IdStatus = Status.Id;
			}
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (AdditionalData.ContainsKey(@"client.idclient"))
			{
				Client = new Client
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


		public string GetBandWidth()
		{
			var result = BandWidth;
			var k = 1000;
			string[] sizes = { @"MB", @"GB", @"TB", @"PB", @"EB", @"ZB", @"YB" };
			var i = (int)Math.Floor(Math.Log(result) / Math.Log(k));

			if (!result.IsGreaterThanZero())
			{
				return @"0 MB";
			}

			for (int j = 0; j < i; j++)
			{
				result= result/k;
			}

			return result.ToString(CultureInfo.InvariantCulture) + ' ' + sizes[i];
		}
	}
}