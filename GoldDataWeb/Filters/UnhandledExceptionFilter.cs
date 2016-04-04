using System.Web.Mvc;
using System.Web.Routing;

namespace GoldDataWeb.Filters
{
	public class UnhandledExceptionFilter : FilterAttribute, IExceptionFilter
	{
		public void OnException(ExceptionContext filterContext)
		{
			if (filterContext.Exception != null)
			{
				filterContext.Controller.TempData[@"TypeExeption"] = filterContext.Exception.GetType();
				filterContext.Controller.TempData[@"MessageExeption"] = filterContext.Exception.Message;

				filterContext.ExceptionHandled = true;
				filterContext.HttpContext.Response.Clear();
				filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

				filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
				{
					Controller = @"Error",
					Action = @"Index"
				}));
			}
		}
	}

}