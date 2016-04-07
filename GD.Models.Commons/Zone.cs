using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GD.Models.Commons
{
	public class Zone
	{
		[JsonProperty(@"idzone")]
		public int Id { get; set; }

		[JsonProperty(@"name")]
		public string Name { get; set; }

		[JsonProperty(@"city")]
		public City City { get; set; }

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



		[JsonProperty(@"idcity")]
		public int IdCity { get; set; }

		[JsonProperty(@"idstatus")]
		public int IdStatus { get; set; }

		[JsonExtensionData]
		public readonly IDictionary<string, JToken> AdditionalData;

		public Zone()
		{
			AdditionalData = new Dictionary<string, JToken>();
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (AdditionalData.ContainsKey(@"city.idcity"))
			{
				City = new City
				{
					Id = (int)AdditionalData[@"city.idcity"]
					//Name = AdditionalData[@"country.name"].ToString(),
					//Aka = AdditionalData[@"country.aka"].ToString(),
					//ClientNumber = (int)AdditionalData[@"country.clientnumber"]
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