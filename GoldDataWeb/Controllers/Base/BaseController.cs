using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GD.Models.Commons;
using GD.Models.Commons.Utilities;
using GoldDataWeb.Filters;
using GoldDataWeb.Models;
using GoldDataWeb.Providers;
using GoldDataWeb.Providers.Services;

namespace GoldDataWeb.Controllers.Base
{
	[UnhandledExceptionFilter]
	public class BaseController : Controller
	{
		private AuthService _authService;
		private Service<User> _userService;
		private Service<Rol> _rolService;
		private Service<Country> _countryService;
		private Service<AccessType> _accessTypeService;
		private Service<Material> _materialService;
		private Service<ServiceType> _serviceTypeService;
		private Service<EntityType> _entityTypeService;
		private Service<EntityContact> _entityContactService;
		private Service<EntityChannel> _entityChannelService;
		private Service<EntityChannelType> _entityChannelTypeService;
		private Service<Position> _positionService;
		private Service<OrderStatus> _orderStatusService;
		private Service<OrderShotType> _orderShotTypeService;
		private Service<OrderShot> _orderShotService;
		private Service<ClientType> _clientTypeService;
		private Service<Client> _clientService;
		private Service<RadioBase> _radioBaseService;
		private Service<Order> _orderService;
		private Service<State> _stateService;
		private Service<City> _cityService;
		private Service<Zone> _zoneService;
		private Service<Site> _siteService;
		private Service<SiteSchedule> _siteScheduleService;
		private Service<SiteAccessType> _siteAccessTypeService;
		private Service<LineSight> _lineSightService;
		private Service<OrderMaterial> _orderMaterialService;
		private Service<OrderFlow> _orderFlowService;

		public AuthService AuthService => _authService ?? (_authService = new AuthService());
		public Service<User> UserService => _userService ?? (_userService = new Service<User>(RestfulServiceProvider.UserUrlApi, GetTokenAuth()));
		public Service<Rol> RolService => _rolService ?? (_rolService = new Service<Rol>(RestfulServiceProvider.RolUrlApi, GetTokenAuth()));
		public Service<Country> CountryService => _countryService ?? (_countryService = new Service<Country>(RestfulServiceProvider.CountryUrlApi, GetTokenAuth()));
		public Service<AccessType> AccessTypeService => _accessTypeService ?? (_accessTypeService = new Service<AccessType>(RestfulServiceProvider.AccessTypeUrlApi, GetTokenAuth()));
		public Service<Material> MaterialService => _materialService ?? (_materialService = new Service<Material>(RestfulServiceProvider.MaterialUrlApi, GetTokenAuth()));
		public Service<ServiceType> ServiceTypeService => _serviceTypeService ?? (_serviceTypeService = new Service<ServiceType>(RestfulServiceProvider.ServiceTypeUrlApi, GetTokenAuth()));
		public Service<EntityType> EntityTypeService => _entityTypeService ?? (_entityTypeService = new Service<EntityType>(RestfulServiceProvider.EntityTypeUrlApi, GetTokenAuth()));
		public Service<EntityContact> EntityContactService => _entityContactService ?? (_entityContactService = new Service<EntityContact>(RestfulServiceProvider.EntityContactUrlApi, GetTokenAuth()));
		public Service<EntityChannel> EntityChannelService => _entityChannelService ?? (_entityChannelService = new Service<EntityChannel>(RestfulServiceProvider.EntityChannelUrlApi, GetTokenAuth()));
		public Service<EntityChannelType> EntityChannelTypeService => _entityChannelTypeService ?? (_entityChannelTypeService = new Service<EntityChannelType>(RestfulServiceProvider.EntityChannelTypeUrlApi, GetTokenAuth()));
		public Service<Position> PositionService => _positionService ?? (_positionService = new Service<Position>(RestfulServiceProvider.PositionUrlApi, GetTokenAuth()));
		public Service<OrderStatus> OrderStatusService => _orderStatusService ?? (_orderStatusService = new Service<OrderStatus>(RestfulServiceProvider.OrderStatusUrlApi, GetTokenAuth()));
		public Service<OrderShotType> OrderShotTypeService => _orderShotTypeService ?? (_orderShotTypeService = new Service<OrderShotType>(RestfulServiceProvider.OrderShotTypeUrlApi, GetTokenAuth()));
		public Service<OrderShot> OrderShotService => _orderShotService ?? (_orderShotService = new Service<OrderShot>(RestfulServiceProvider.OrderShotUrlApi, GetTokenAuth()));
		public Service<RadioBase> RadioBaseService => _radioBaseService ?? (_radioBaseService = new Service<RadioBase>(RestfulServiceProvider.RadioBaseUrlApi, GetTokenAuth()));
		public Service<ClientType> ClientTypeService => _clientTypeService ?? (_clientTypeService = new Service<ClientType>(RestfulServiceProvider.ClientTypeUrlApi, GetTokenAuth()));
		public Service<Client> ClientService => _clientService ?? (_clientService = new Service<Client>(RestfulServiceProvider.ClientUrlApi, GetTokenAuth()));
		public Service<Order> OrderService => _orderService ?? (_orderService = new Service<Order>(RestfulServiceProvider.OrderUrlApi, GetTokenAuth()));
		public Service<State> StateService => _stateService ?? (_stateService = new Service<State>(RestfulServiceProvider.StateUrlApi, GetTokenAuth()));
		public Service<City> CityService => _cityService ?? (_cityService = new Service<City>(RestfulServiceProvider.CityUrlApi, GetTokenAuth()));
		public Service<Zone> ZoneService => _zoneService ?? (_zoneService = new Service<Zone>(RestfulServiceProvider.ZoneUrlApi, GetTokenAuth()));
		public Service<Site> SiteService => _siteService ?? (_siteService = new Service<Site>(RestfulServiceProvider.SiteUrlApi, GetTokenAuth()));
		public Service<SiteSchedule> SiteScheduleService => _siteScheduleService ?? (_siteScheduleService = new Service<SiteSchedule>(RestfulServiceProvider.SiteScheduleUrlApi, GetTokenAuth()));
		public Service<SiteAccessType> SiteAccessTypeService => _siteAccessTypeService ?? (_siteAccessTypeService = new Service<SiteAccessType>(RestfulServiceProvider.SiteAccessTypeUrlApi, GetTokenAuth()));
		public Service<LineSight> LineSightService => _lineSightService ?? (_lineSightService = new Service<LineSight>(RestfulServiceProvider.LineSightUrlApi, GetTokenAuth()));
		public Service<OrderMaterial> OrderMaterialService => _orderMaterialService ?? (_orderMaterialService = new Service<OrderMaterial>(RestfulServiceProvider.OrderMaterialUrlApi, GetTokenAuth()));
		public Service<OrderFlow> OrderFlowService => _orderFlowService ?? (_orderFlowService = new Service<OrderFlow>(RestfulServiceProvider.OrderFlowServiceUrlApi, GetTokenAuth()));


		public Auth ValidateRememberAuth()
		{
			HttpCookie authCookie = Request.Cookies[@"LoginCookie"];
			if (authCookie != null)
			{
				//Extract the forms authentication cookie
				FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
				if (authTicket != null)
				{
					var data = JsonHelper.Deserialize<Auth>(authTicket.UserData);
					if (data.RememberMe)
					{
						return new Auth
						{
							UserName = data.UserName,
							Password = data.Password,
							RememberMe = data.RememberMe
						};
					}
				}
			}
			return new Auth();
		}

		public Auth GetAuthData()
		{
			var data = (UserIdentity)User;
			return data.Auth;
		}

		public string GetTokenAuth()
		{
			var data = (UserIdentity)User;
			return data.Auth.AccessToken;
		}

		public void CreateAuthCookie(Auth auth)
		{
			var authTicket = new FormsAuthenticationTicket(
				1,
				auth.UserId.ToString(),
				DateTime.Now,
				DateTime.Now.AddHours(5),
				auth.RememberMe,
				auth.ToJson(),
				FormsAuthentication.FormsCookiePath);

			//encrypt the ticket and add it to a cookie
			HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
			Response.Cookies.Add(cookie);
		}

		public void CreateLoginCookie(Auth auth)
		{
			if (auth.RememberMe)
			{
				var authTicket = new FormsAuthenticationTicket(
					1,
					@"Login",
					DateTime.Now,
					DateTime.Now.AddMonths(1),
					auth.RememberMe,
					auth.ToJson(),
					FormsAuthentication.FormsCookiePath);

				var cookie = new HttpCookie(@"LoginCookie", FormsAuthentication.Encrypt(authTicket));
				Response.Cookies.Add(cookie);
			}
			else
			{
				DeleteCookie(@"LoginCookie");
			}
		}

		public void DeleteCookie(string name)
		{
			var c = new HttpCookie(name) { Expires = DateTime.Now.AddDays(-1) };
			Response.Cookies.Add(c);
		}
	}
}