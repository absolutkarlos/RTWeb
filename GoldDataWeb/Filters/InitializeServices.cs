using System.Web;
using System.Web.Mvc;

namespace GoldDataWeb.Filters
{
	public class InitializeServices : ActionFilterAttribute, IActionFilter
	{
		void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (HttpContext.Current.User.Identity.IsAuthenticated)
			{
				
			}
			OnActionExecuting(filterContext);
		}
	}
}
