using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GD.Models.Commons
{
	public class RolAction
	{
		[JsonProperty(@"idrolaction")]
		public int Id { get; set; }

		[JsonProperty(@"rol")]
		public Rol Rol { get; set; }

		[JsonProperty(@"action")]
		public Action Action { get; set; }


		[JsonProperty(@"idrol")]
		public int IdRol { get; set; }

		[JsonProperty(@"idaction")]
		public int IdAction { get; set; }


		[JsonExtensionData]
		public readonly IDictionary<string, JToken> AdditionalData;

		public RolAction()
		{
			AdditionalData = new Dictionary<string, JToken>();
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (AdditionalData.ContainsKey(@"rol.idrol"))
			{
				Rol = new Rol
				{
					Id = (int)AdditionalData[@"rol.idrol"]
					//Name = AdditionalData[@"rol.name"].ToString()
				};
			}

			if (AdditionalData.ContainsKey(@"action.idaction"))
			{
				Action = new Action
				{
					Id = (int)AdditionalData[@"action.idaction"]
					//Name = AdditionalData[@"action.name"].ToString()
				};
			}

			AdditionalData.Clear();
		}
	}
}