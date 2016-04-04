using System.Web.Mvc;

namespace GoldDataWeb.Controllers
{
	[AllowAnonymous]
	public class ErrorController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Color = @"rgb(72, 72, 72);";
			//ViewBag.TypeException = TempData[@"TypeException"];
			return View();
		}

		public ActionResult UnauthorizedAccess()
		{
			ViewBag.Color = @"rgb(72, 72, 72);";
			return View();
		}

		public ActionResult TimeOut()
		{
			ViewBag.Color = @"rgb(72, 72, 72);";
			return View();
		}
	}
}