using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GD.Models.Commons
{
	public class RadioBaseLineSight
	{
		[JsonProperty(@"idradiobaselinesight")]
		public int Id { get; set; }

		[JsonProperty(@"linesight")]
		public LineSight LineSight { get; set; }

		[JsonProperty(@"radiobase")]
		public RadioBase RadioBase { get; set; }

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


		[JsonProperty(@"idlinesight")]
		public int IdLineSight { get; set; }

		[JsonProperty(@"idradiobase")]
		public int IdRadioBase { get; set; }

		[JsonProperty(@"idstatus")]
		public int IdStatus { get; set; }


		[JsonExtensionData]
		public readonly IDictionary<string, JToken> AdditionalData;

		public RadioBaseLineSight()
		{
			AdditionalData = new Dictionary<string, JToken>();
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (AdditionalData.ContainsKey(@"linesight.idlinesight"))
			{
				LineSight = new LineSight
				{
					Id = (int) AdditionalData[@"linesight.idlinesight"]
				};
			}

			if (AdditionalData.ContainsKey(@"radiobase.idradiobase"))
			{
				RadioBase = new RadioBase
				{
					Id = (int) AdditionalData[@"radiobase.idradiobase"]
					//Name = AdditionalData[@"radiobase.name"].ToString()
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