using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GD.Models.Commons.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GD.Models.Commons
{
	public class Client
	{
		[JsonProperty(@"idclient")]
		public long Id { get; set; }

		[JsonProperty(@"clienttype")]
		public ClientType ClientType { get; set; }

		[JsonProperty(@"country")]
		public Country Country { get; set; }

		[JsonProperty(@"state")]
		public State State { get; set; }

		[JsonProperty(@"city")]
		public City City { get; set; }

		[JsonProperty(@"zone")]
		public Zone Zone { get; set; }

		[JsonProperty(@"businessname")]
		public string BusinessName { get; set; }

		[JsonProperty(@"legalname")]
		public string LegalName { get; set; }

		[JsonProperty(@"ruc")]
		public string Ruc { get; set; }

		[JsonProperty(@"addressref")]
		public string AddressRef { get; set; }

		[JsonProperty(@"sitenumber")]
		public string SiteNumber { get; set; }

		[JsonProperty(@"clientnumber")]
		public int? ClientNumber { get; set; }

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

		public IEnumerable<EntityContact> ListEntityContact { get; set; }

		[JsonProperty(@"idcountry")]
		public int IdCountry { get; set; }

		[JsonProperty(@"idstate")]
		public int IdState { get; set; }

		[JsonProperty(@"idcity")]
		public int IdCity { get; set; }

		[JsonProperty(@"idzone")]
		public int IdZone { get; set; }

		[JsonProperty(@"idclienttype")]
		public int IdClientType { get; set; }

		[JsonProperty(@"idstatus")]
		public int IdStatus { get; set; }




		[JsonExtensionData]
		public readonly IDictionary<string, JToken> AdditionalData;

		public Client()
		{
			AdditionalData = new Dictionary<string, JToken>();
		}

		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			if ((Country != null) && (Country.Id.IsGreaterThanZero()))
			{
				IdCountry = Country.Id;
			}

			if ((State != null) && (State.Id.IsGreaterThanZero()))
			{
				IdState = State.Id;
			}

			if ((City != null) && (City.Id.IsGreaterThanZero()))
			{
				IdCity = City.Id;
			}

			if ((Zone != null) && (Zone.Id.IsGreaterThanZero()))
			{
				IdZone = Zone.Id;
			}

			if ((ClientType != null) && (ClientType.Id.IsGreaterThanZero()))
			{
				IdClientType = ClientType.Id;
			}

			if ((Status != null) && (Status.Id.IsGreaterThanZero()))
			{
				IdStatus = Status.Id;
			}
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (AdditionalData.ContainsKey(@"country.idcountry"))
			{
				Country = new Country
				{
					Id = (int)AdditionalData[@"country.idcountry"],
					Name = AdditionalData.ContainsKey(@"country.name") ? AdditionalData[@"country.name"].ToString() : string.Empty,
					Aka = AdditionalData.ContainsKey(@"country.aka") ? AdditionalData[@"country.aka"].ToString() : string.Empty,
					ClientNumber = AdditionalData.ContainsKey(@"country.clientnumber") ? (int)AdditionalData[@"country.clientnumber"] : 0
				};
			}

			if (AdditionalData.ContainsKey(@"state.idstate"))
			{
				State = new State
				{
					Id = (int)AdditionalData[@"state.idstate"],
					Name = AdditionalData.ContainsKey(@"state.name") ? AdditionalData[@"state.name"].ToString() : string.Empty
				};
			}

			if (AdditionalData.ContainsKey(@"city.idcity"))
			{
				City = new City
				{
					Id = (int)AdditionalData[@"city.idcity"],
					Name = AdditionalData.ContainsKey(@"city.name") ? AdditionalData[@"city.name"].ToString() : string.Empty
				};
			}

			if (AdditionalData.ContainsKey(@"zone.idzone"))
			{
				Zone = new Zone
				{
					Id = (int)AdditionalData[@"zone.idzone"],
					Name = AdditionalData.ContainsKey(@"zone.name") ? AdditionalData[@"zone.name"].ToString() : string.Empty
				};
			}

			if (AdditionalData.ContainsKey(@"clienttype.idclienttype"))
			{
				ClientType = new ClientType
				{
					Id = (int)AdditionalData[@"clienttype.idclienttype"],
					Name = AdditionalData.ContainsKey(@"clienttype.name") ? AdditionalData[@"clienttype.name"].ToString() : string.Empty
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

		public enum Type
		{
			Natural = 1,
			Juridico = 2
		}
	}
}