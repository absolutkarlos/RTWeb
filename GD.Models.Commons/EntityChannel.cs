using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using GD.Models.Commons.Utilities;

namespace GD.Models.Commons
{
	public class EntityChannel
	{
		[JsonProperty(@"identitychannel")]
		public int Id { get; set; }

		[JsonProperty(@"contact")]
		[Required]
		public EntityContact Contact { get; set; }

		[Required]
		[JsonProperty(@"entitychanneltype")]
		public EntityChannelType EntityChannelType { get; set; }

		[Required]
		[JsonProperty(@"entitytype")]
		public EntityType EntityType { get; set; }

		[JsonProperty(@"channel")]
		public string Channel { get; set; }

		[JsonProperty(@"description")]
		public string Description { get; set; }

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


		[JsonProperty(@"identitytype")]
		public int IdEntityType { get; set; }

		[JsonProperty(@"identitycontact")]
		public int IdEntityContact { get; set; }

		[JsonProperty(@"idstatus")]
		public int IdStatus { get; set; }

		[JsonProperty(@"identitychanneltype")]
		public int IdEntityChannelType { get; set; }


		[JsonExtensionData]
		public readonly IDictionary<string, JToken> AdditionalData;

		public EntityChannel()
		{
			AdditionalData = new Dictionary<string, JToken>();
		}

		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			if ((Contact != null) && (Contact.Id.IsGreaterThanZero()))
			{
				IdEntityContact = Contact.Id;
			}

			if ((EntityType != null) && (EntityType.Id.IsGreaterThanZero()))
			{
				IdEntityType = EntityType.Id;
			}

			if ((EntityChannelType != null) && (EntityChannelType.Id.IsGreaterThanZero()))
			{
				IdEntityChannelType = EntityChannelType.Id;
			}

			if ((Status != null) && (Status.Id.IsGreaterThanZero()))
			{
				IdStatus = Status.Id;
			}
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (AdditionalData.ContainsKey(@"contact.idcontact"))
			{
				Contact = new EntityContact
				{
					Id = (int)AdditionalData[@"contact.idcontact"]
					//Name = AdditionalData[@"contact.name"].ToString()
				};
			}

			//if (AdditionalData.ContainsKey(@"entitychanneltype.identitychanneltype"))
			//{
			//	EntityChannelType = new EntityChannelType
			//	{
			//		Id = (int)AdditionalData[@"entitychanneltype.identitychanneltype"]
			//	};
			//}

			if (AdditionalData.ContainsKey(@"entitytype.identitytype"))
			{
				EntityType = new EntityType
				{
					Id = (int)AdditionalData[@"entitytype.identitytype"]
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