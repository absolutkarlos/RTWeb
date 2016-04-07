using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace GD.Models.Commons
{
	public class Auth
	{
		[JsonProperty(@"userId")]
		public long UserId { get; set; }

		[Required]
		[JsonProperty(@"UserName")]
		public string UserName { get; set; }

		[Required]
		[JsonProperty(@"Password")]
		public string Password { get; set; }

		[JsonProperty(@"access_token")]
		public string AccessToken { get; set; }

		[JsonProperty(@"token_type")]
		public string TokenType { get; set; }

		[JsonProperty(@"expires_in")]
		public string ExpiresIn { get; set; }

		[JsonProperty(@".issued")]
		public string Issued { get; set; }

		[JsonProperty(@".expires")]
		public DateTime? Expires { get; set; }

		[JsonProperty(@"rememberme")]
		public bool RememberMe { get; set; }

		[JsonProperty(@"grant_type")]
		public string grant_type { get; set; }
	}
}
