using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GD.Models.Commons;
using GD.Models.Commons.Utilities;
using GoldDataWeb.Controllers.Base;
using GoldDataWeb.Filters;
using GoldDataWeb.Models;
using RestSharp;

namespace GoldDataWeb.Controllers
{
	[UnhandledExceptionFilter]
	public class HomeController : BaseController
	{
		public ActionResult Index()
		{
			ResponseService<Order> order = null;
			var responseUser = UserService.GetById(GetAuthData().UserId);
			((UserIdentity)User).Rol = RolService.GetById(responseUser.Data.IdRol).Data;
			var responseOrders = OrderService.Execute<IEnumerable<Order>>(@"getbyuser", Method.GET, responseUser.Data.Id.ToString());
			var firstOrDefault = responseOrders.Data.FirstOrDefault();
			if (firstOrDefault != null)
			{
				order = (!((UserIdentity)User).IsRole(Rol.Type.Ventas))
					? OrderService.Execute(@"getinfo", Method.GET, firstOrDefault.Id.ToString())
					: null;
			}

			var viewModel = new HomeViewModel
			{
				Orders = responseOrders.Data,
				User = responseUser.Data,
				Order = order?.Data
			};

			return View(viewModel);
		}

		public ActionResult OrderPanel()
		{
			var responseOrders = OrderService.Execute<IEnumerable<Order>>(@"getbyuser", Method.GET, GetAuthData().UserId.ToString());

			return PartialView("_OrderPanel", responseOrders.Data);
		}

		public ActionResult InfoOrderPanel(int orderId)
		{
			var responseOrder = OrderService.Execute(@"getinfo", Method.GET, orderId.ToString());

			return PartialView("_WizardTabInfo", responseOrder.Data);
		}

		[HttpPost]
		public ContentResult UploadFiles(UploadFileViewModel viewModel)
		{
			var r = new List<UploadFilesResultViewModel>();

			for (int i = 0; i < Request.Files.Count; i++)
			{
				HttpPostedFileBase hpf = Request.Files[i];
				if (hpf != null && hpf.ContentLength == 0)
					continue;

				string path = Path.GetFileName(viewModel.OrderNumber) + "_" + viewModel.OrderShotCount + @"." + Path.GetFileName(hpf.ContentType);
				string savedFileName = Path.Combine(Server.MapPath("~/App_Data"), path);
				//string savedFileName = Path.Combine(Server.MapPath("C:\"inetpub\"wwwroot\"RtSurvey\"OrderShots"), path);

				hpf.SaveAs(savedFileName);

				OrderShotService.Insert(new OrderShot
				{
					Order = new Order
					{
						Id = int.Parse(viewModel.OrderId)
					},
					IdOrderShotType = viewModel.OrderShotType,
					Comment = viewModel.Comment,
					ShotPath = path,
					Status = new Status
					{
						Id = (int)Status.Type.Activo
					},
					CreateAt = DateTime.Now,
					CreateBy = short.Parse(GetAuthData().UserId.ToString())
				});

				r.Add(new UploadFilesResultViewModel()
				{
					Name = hpf.FileName,
					Length = hpf.ContentLength,
					Type = hpf.ContentType
				});
			}

			// Returns json
			return Content("{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + $"{r[0].Length} bytes" + "\"}", "application/json");
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}