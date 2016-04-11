using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace GD.Models.Commons
{
	public class AccessType
	{
		[JsonProperty(@"idaccesstype")]
		public int Id { get; set; }

		[JsonProperty(@"name")]
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

		public static string GetConcatAccessTypes(List<SiteAccessType> siteAccessTypes)
		{
			var concatAccessTypes = string.Empty;
			for (int i = 0; i < siteAccessTypes.Count; i++)
			{
				if (i > 0)
				{
					concatAccessTypes += ", ";
				}
				var element = siteAccessTypes.ElementAt(i);
				concatAccessTypes += element.AccessType.Name;
			}
			return concatAccessTypes;
		}
	}
}