using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GD.Models.Commons.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GD.Models.Commons
{
	public class SiteAccessType
	{
		[JsonProperty(@"idsiteaccesstype")]
		public int Id { get; set; }

		[JsonProperty(@"site")]
		public Site Site { get; set; }

		[JsonProperty(@"accesstype")]
		public AccessType AccessType { get; set; }

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

		[JsonProperty(@"idaccesstype")]
		public int IdAccessType { get; set; }

		[JsonProperty(@"idstatus")]
		public int IdStatus { get; set; }


		[JsonExtensionData]
		public readonly IDictionary<string, JToken> AdditionalData;

		public SiteAccessType()
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

			if ((AccessType != null) && (AccessType.Id.IsGreaterThanZero()))
			{
				IdAccessType = AccessType.Id;
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
					//Name = AdditionalData[@"site.name"].ToString()
				};
			}

			if (AdditionalData.ContainsKey(@"accesstype.idaccesstype"))
			{
				AccessType = new AccessType
				{
					Id = (int)AdditionalData[@"accesstype.idaccesstype"]
					//Name = AdditionalData[@"accesstype.name"].ToString()
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