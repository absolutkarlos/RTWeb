using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GD.Models.Commons
{
	public class RadioBase
	{
		[JsonProperty(@"idradiobase")]
		public int Id { get; set; }

		[JsonProperty(@"name")]
		public string Name { get; set; }

		[JsonProperty(@"country")]
		public Country Country { get; set; }

		[JsonProperty(@"latitude")]
		public string Latitude { get; set; }

		[JsonProperty(@"longitude")]
		public string Longitude { get; set; }

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


		[JsonProperty(@"idcountry")]
		public int IdCountry { get; set; }

		[JsonProperty(@"idstatus")]
		public int IdStatus { get; set; }


		[JsonExtensionData]
		public readonly IDictionary<string, JToken> AdditionalData;

		public RadioBase()
		{
			AdditionalData = new Dictionary<string, JToken>();
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (AdditionalData.ContainsKey(@"country.idcountry"))
			{
				Country = new Country
				{
					Id = (int)AdditionalData[@"country.idcountry"]
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