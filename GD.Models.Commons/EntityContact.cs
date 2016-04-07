using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GD.Models.Commons.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GD.Models.Commons
{
	public class EntityContact
	{
		[JsonProperty(@"identitycontact")]
		public int Id { get; set; }

		[JsonProperty(@"identity")]
		public int IdEntity { get; set; }

		[JsonProperty(@"entitytype")]
		public EntityType EntityType { get; set; }

		[JsonProperty(@"position")]
		public Position Position { get; set; }

		[JsonProperty(@"idcontactrelated")]
		public int? IdContactRelated { get; set; }

		[JsonProperty(@"country")]
		public Country Country { get; set; }

		[JsonProperty(@"state")]
		public State State { get; set; }

		[JsonProperty(@"city")]
		public City City { get; set; }

		[JsonProperty(@"zone")]
		public Zone Zone { get; set; }

		[JsonProperty(@"entitycontactname")]
		public string Name { get; set; }

		[JsonProperty(@"lastname")]
		public string LastName { get; set; }

		[JsonProperty(@"datestart")]
		public DateTime? StartDate { get; set; }

		[JsonProperty(@"dateend")]
		public DateTime? EndDate { get; set; }

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


		public IEnumerable<EntityChannel> ListEntityChannels { get; set; }


		[JsonProperty(@"idcountry")]
		public int IdCountry { get; set; }

		[JsonProperty(@"idstate")]
		public int IdState { get; set; }

		[JsonProperty(@"idcity")]
		public int IdCity { get; set; }

		[JsonProperty(@"idzone")]
		public int IdZone { get; set; }

		[JsonProperty(@"identitytype")]
		public int IdEntityType { get; set; }

		[JsonProperty(@"idstatus")]
		public int IdStatus { get; set; }

		[JsonProperty(@"idposition")]
		public int IdPosition { get; set; }



		[JsonExtensionData]
		public readonly IDictionary<string, JToken> AdditionalData;

		public EntityContact()
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

			if ((EntityType != null) && (EntityType.Id.IsGreaterThanZero()))
			{
				IdEntityType = EntityType.Id;
			}

			if ((Position != null) && (Position.Id.IsGreaterThanZero()))
			{
				IdPosition = Position.Id;
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
					Aka = AdditionalData.ContainsKey(@"country.aka") ? AdditionalData[@"country.aka"].ToString() : string.Empty
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

			if (AdditionalData.ContainsKey(@"position.idposition"))
			{
				Position = new Position
				{
					Id = (int)AdditionalData[@"position.idposition"],
					Name = AdditionalData.ContainsKey(@"position.name") ? AdditionalData[@"position.name"].ToString() : string.Empty
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