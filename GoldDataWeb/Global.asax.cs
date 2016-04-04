using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using GD.Models.Commons;
using GD.Models.Commons.Utilities;
using GoldDataWeb.Models;

namespace GoldDataWeb
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		protected void Application_PostAuthenticateRequest()
		{
			HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
			if (authCookie != null)
			{
				//Extract the forms authentication cookie
				FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

				// If caching roles in userData field then extract
				if (authTicket != null)
				{
					// Create the IIdentity instance
					IIdentity id = new FormsIdentity(authTicket);

					// Create the IPrinciple instance
					var user = new UserIdentity(id)
					{
						Auth = JsonHelper.Deserialize<Auth>(authTicket.UserData)
					};

					// Set the context user 
					Context.User = user;
				}
			}
		}
	}
}
