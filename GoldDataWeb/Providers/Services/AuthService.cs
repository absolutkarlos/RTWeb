using GD.Models.Commons;
using GD.Models.Commons.Utilities;

namespace GoldDataWeb.Providers.Services
{
	public class AuthService
	{
		public RestfulServiceProvider Service;

		public AuthService()
		{
			Service = new RestfulServiceProvider(RestfulServiceProvider.AuthUrlApi);
		}

		public Auth Auth(Auth auth)
		{
			auth.grant_type = RestfulServiceProvider.GrantType;
			Service.AddObjectToRequest(auth);
			return Service.Post<Auth>();
		}

		public Auth RefreshToken(Auth auth)
		{
			auth.grant_type = RestfulServiceProvider.GrantTypeRefresh;
			Service.AddObjectToRequest(auth);
			return Service.Post<Auth>();
		}
	}
}