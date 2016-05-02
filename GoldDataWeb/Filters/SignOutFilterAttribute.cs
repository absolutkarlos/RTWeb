using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace GoldDataWeb.Filters
{
	public class SignOutFilterAttribute : ActionFilterAttribute, IActionFilter
	{
		void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (HttpContext.Current.User.Identity.IsAuthenticated)
			{
				FormsAuthentication.SignOut();
				filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
				{
					Controller = @"Login",
					Action = @"Index"
				}));
			}
			OnActionExecuting(filterContext);
		}
	}
}
