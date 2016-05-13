using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using GD.Models.Commons.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GD.Models.Commons
{
	public class User
	{
		[JsonProperty(@"iduser")]
		public long Id { get; set; }

		[Required]
		[JsonProperty(@"username")]
		public string UserName { get; set; }

		[Required]
		[JsonProperty(@"userpassword")]
		public string Password { get; set; }

		[JsonProperty(@"fullname")]
		public string Name { get; set; }

		[JsonProperty(@"lastname")]
		public string LastName { get; set; }

		[JsonProperty(@"identification")]
		public string Identification { get; set; }

		[JsonProperty(@"address")]
		public string BillingAddress { get; set; }

		[JsonProperty(@"zipcode")]
		public string ZipCode { get; set; }

		[JsonProperty(@"phonenumber")]
		public string PhoneNumber { get; set; }

		[JsonProperty(@"mail")]
		public string Email { get; set; }

		[JsonProperty(@"datedeactivated")]
		public DateTime? DateDeActivated { get; set; }

		[JsonProperty(@"dateactivated")]
		public DateTime? DateActivated { get; set; }

		[JsonProperty(@"rol")]
		public Rol Rol { get; set; }

		[JsonProperty(@"usertoken")]
		public string UserToken { get; set; }

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


		[JsonProperty(@"idrol")]
		public int IdRol { get; set; }

		[JsonProperty(@"idstatus")]
		public int IdStatus { get; set; }

		public bool IsRole(Rol.Type type)
		{
			return IdRol == (int)type;
		}

		[JsonExtensionData]
		public readonly IDictionary<string, JToken> AdditionalData;

		public User()
		{
			AdditionalData = new Dictionary<string, JToken>();
		}

		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			if ((Rol != null) && (Rol.Id.IsGreaterThanZero()))
			{
				IdRol = Rol.Id;
			}

			if ((Status != null) && (Status.Id.IsGreaterThanZero()))
			{
				IdStatus = Status.Id;
			}
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (AdditionalData.ContainsKey(@"rol.idrol"))
			{
				Rol = new Rol
				{
					Id = (int)AdditionalData[@"rol.idrol"],
					Name = AdditionalData.ContainsKey(@"rol.name") ? AdditionalData[@"rol.name"].ToString() : string.Empty
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
