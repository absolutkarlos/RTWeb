using System;
using System.Collections.Generic;
using System.Configuration;
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
			var responseOrders = OrderService.Execute<IEnumerable<Order>>(@"getbyuser", Method.GET, responseUser.Data.Id.ToString());
			var firstOrDefault = responseOrders.Data.FirstOrDefault();

			if (firstOrDefault != null)
			{
				order = (!UserData.IsRole(Rol.Type.Ventas)) ? OrderService.Execute(@"getinfo", Method.GET, firstOrDefault.Id.ToString()) : null;
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
			var homeViewModel = new HomeViewModel
			{
				Order = responseOrder.Data
			};

			return PartialView("_WizardTabInfo", homeViewModel);
		}

		public ActionResult InspectionPanel(int orderId)
		{
			var responseOrder = OrderService.Execute(@"getinfo", Method.GET, orderId.ToString());
			var homeViewModel = new HomeViewModel
			{
				Order = responseOrder.Data
			};
			return PartialView("_WizardTabInspection", homeViewModel);
		}

		public ActionResult InstalationPanel(int orderId)
		{
			var responseOrder = OrderService.Execute(@"getinfo", Method.GET, orderId.ToString());
			var homeViewModel = new HomeViewModel
			{
				Order = responseOrder.Data
			};
			return PartialView("_WizardTabInstalation", homeViewModel);
		}

		[HttpPost]
		public JsonResult ExistingClientValidate(Client client)
		{
			var responseOrder = ClientService.Execute<int>(@"ValidateByRuc", Method.POST, null, client.ToJson());
			return Json(responseOrder);
		}

		public ActionResult UserProfile()
		{
			var responseUser = UserService.GetById(GetAuthData().UserId.ToString());

			User user = responseUser.Data;
			user.Rol = RolService.GetById(user.IdRol.ToString()).Data;

			return View(user);
		}

		[HttpPost]
		public JsonResult UpdateProfile(User user)
		{
			var responseUser = UserService.GetById(GetAuthData().UserId.ToString());

			user.Id = long.Parse(GetAuthData().UserId.ToString());
			user.Rol = RolService.GetById(responseUser.Data.IdRol).Data;
			
			user.UpdateAt = DateTime.Now;
			user.UpdateBy = short.Parse(GetAuthData().UserId.ToString());

			user.Password = !string.IsNullOrWhiteSpace(user.Password) ? (new Utility()).Encrypt(user.Password) : responseUser.Data.Password;

			return Json(UserService.Update(user));
		}

		[HttpPost]
		public ContentResult UploadFiles(UploadFileViewModel viewModel)
		{
			var r = new List<UploadFilesResultViewModel>();

			if (viewModel.OrderShotType == (int)OrderShot.Type.LineSight)
			{
				var newSite = new Site
				{
					Id = int.Parse(viewModel.SiteId),
					LinkType = viewModel.LinkType,
					UpdateBy = short.Parse(GetAuthData().UserId.ToString())
				};

				SiteService.Execute(@"UpdateLinkType", Method.PUT, null, newSite.ToJson());

				var lineSight = new LineSight
				{
					RadioBase = new RadioBase
					{
						Id = int.Parse(viewModel.RadioBaseId)
					},
					Site = new Site
					{
						Id = int.Parse(viewModel.SiteId)
					},
					Distance = int.Parse(viewModel.Distance),
					CreateAt = DateTime.Now,
					CreateBy = short.Parse(GetAuthData().UserId.ToString()),
					Status = new Status
					{
						Id = (int)Status.Type.Activo
					}
				};

				LineSightService.Insert(lineSight);
			}

			viewModel.OrderShotCount = OrderShotService.Execute<List<OrderShot>>(@"GetByOrder", Method.GET, viewModel.OrderId).Data.Count;

			string path = Path.GetFileName(viewModel.OrderNumber) + "_" + (viewModel.OrderShotCount + 1);

			for (int i = 0; i < Request.Files.Count; i++)
			{
				HttpPostedFileBase hpf = Request.Files[i];
				if (hpf != null && hpf.ContentLength == 0)
					continue;

				path += @"." + Path.GetFileName(hpf.ContentType);

				hpf.SaveAs(Path.Combine(Server.MapPath("~/App_Data"), path));

				OrderShotService.Insert(new OrderShot
				{
					Order = new Order
					{
						Id = int.Parse(viewModel.OrderId)
					},
					IdOrderShotType = viewModel.OrderShotType,
					Comment = viewModel.Comment,
					ShotPath = Path.Combine(ConfigurationManager.AppSettings[@"UrlOrderShot"], path),
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
	}
}