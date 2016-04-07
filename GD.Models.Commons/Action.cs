using System;
using Newtonsoft.Json;

namespace GD.Models.Commons
{
	public class Action
	{
		[JsonProperty(@"idaction")]
		public int Id { get; set; }

		[JsonProperty(@"name")]
		public string Name { get; set; }

		[JsonProperty(@"code")]
		public string Code { get; set; }

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

		public enum ActionType
		{
			Update,
			GetAll,
			GetById,
			Delete,
			Disconnect,
			LogIn
		}
	}
}