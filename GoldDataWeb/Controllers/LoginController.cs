using System.Web.Mvc;
using System.Web.Security;
using GD.Models.Commons;
using GD.Models.Commons.Utilities;
using GoldDataWeb.Controllers.Base;
using GoldDataWeb.Filters;

namespace GoldDataWeb.Controllers
{
	[UnhandledExceptionFilter]
	public class LoginController : BaseController
	{
		//
		// GET: /Account/Login
		[AllowAnonymous]
		public ActionResult Index()
		{
			CloseSession();
			return View(ValidateRememberAuth());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[AllowAnonymous]
		public ActionResult Login(Auth auth)
		{
			if (ModelState.IsValid)
			{
				var response = AuthService.Auth(auth);
				if (response.UserId.IsGreaterThanZero())
				{
					CreateAuthCookie(response);
					CreateLoginCookie(auth);
					return RedirectToAction(@"Index", @"Home");
				}
			}
			
			return View(@"Index", auth);
		}

		[AllowAnonymous]
		public ActionResult LogOff()
		{
			CloseSession();
			return RedirectToAction(@"Index");
		}

		[HttpPost]
		[Authorize]
		public JsonResult RefreshToken()
		{
			var response = AuthService.RefreshToken(GetAuthData());
			if (response.UserId.IsGreaterThanZero())
			{
				DeleteCookie(@"RefreshToken");
				FormsAuthentication.SignOut();
				CreateAuthCookie(response);
			}

			return Json(new { UpdateToken = response.UserId.IsGreaterThanZero() });
		}
	}
}