using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using GD.Models.Commons.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GD.Models.Commons
{
	public class SiteSchedule
	{
		private string _startTime;
		private string _endTime;

		[JsonProperty(@"idsiteschedule")]
		public int Id { get; set; }

		[JsonProperty(@"site")]
		public Site Site { get; set; }

		[JsonProperty(@"day")]
		public string Day { get; set; }

		public string StartTimeString
		{
			get { return _startTime; }
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					_startTime = value;
					DateTime dateTime = DateTime.ParseExact(value,
										"hh:mm tt", CultureInfo.InvariantCulture);
					StartTime = dateTime.TimeOfDay;
				}
			}
		}

		public string EndTimeString
		{
			get { return _endTime; }
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					_endTime = value;
					DateTime dateTime = DateTime.ParseExact(value,
						"hh:mm tt", CultureInfo.InvariantCulture);
					EndTime = dateTime.TimeOfDay;
				}
			}
		}

		[JsonProperty(@"startime")]
		public TimeSpan? StartTime { get; set; }

		[JsonProperty(@"endtime")]
		public TimeSpan? EndTime { get; set; }

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


		[JsonProperty(@"idsite")]
		public int IdSite { get; set; }

		[JsonProperty(@"idstatus")]
		public int IdStatus { get; set; }

		[JsonExtensionData]
		public readonly IDictionary<string, JToken> AdditionalData;

		public SiteSchedule()
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
					Id = (int)AdditionalData[@"site.idsite"]
				};
			}

			if (AdditionalData.ContainsKey(@"status.idstatus"))
			{
				Status = new Status
				{
					Id = (int)AdditionalData[@"status.idstatus"]
				};
			}

			AdditionalData.Clear();
		}

		public static string GetWekDays(IGrouping<object, SiteSchedule> group)
		{
			string[] weekDays = { @"Lunes", @"Martes", @"Miercoles", @"Jueves", @"Viernes", @"Sabado" };
			var days = string.Empty;
			for (int i = 0; i < group.Count(); i++)
			{
				if ((i > 0) && (i == (group.Count() - 1)))
				{
					days += " y ";
				}
				else if (i > 0)
				{
					days += ", ";
				}
				days += weekDays[int.Parse(group.ElementAt(i).Day)- 1];
			}
			return days;
		}
	}
}