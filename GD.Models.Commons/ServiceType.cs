using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace GD.Models.Commons
{
	public class ServiceType
	{
		[JsonProperty(@"idservicetype")]
		public int Id { get; set; }

		[JsonProperty(@"name")]
		public string Name { get; set; }

		[JsonProperty(@"aka")]
		public string Aka { get; set; }

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

		public static string GetConcatServiceTypes(List<ServiceType> serviceTypes)
		{
			var concatServiceTypes = string.Empty;
			for (int i = 0; i < serviceTypes.Count; i++)
			{
				if (i > 0)
				{
					concatServiceTypes += ", ";
				}
				var element = serviceTypes.ElementAt(i);
				concatServiceTypes += element.Name + @" (" + element.Aka + @")";
			}
			return concatServiceTypes;
		}
	}
}