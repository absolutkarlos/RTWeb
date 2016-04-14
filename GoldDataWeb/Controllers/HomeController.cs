﻿using System;
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
		public ContentResult UploadFiles(UploadFileViewModel viewModel)
		{
			var r = new List<UploadFilesResultViewModel>();

			viewModel.OrderShotCount = OrderShotService.Execute<List<OrderShot>>(@"GetByOrder", Method.GET, viewModel.OrderId).Data.Count;

			for (int i = 0; i < Request.Files.Count; i++)
			{
				HttpPostedFileBase hpf = Request.Files[i];
				if (hpf != null && hpf.ContentLength == 0)
					continue;

				viewModel.OrderShotCount += (i + 1);

				string path = Path.GetFileName(viewModel.OrderNumber) + "_" + viewModel.OrderShotCount + @"." + Path.GetFileName(hpf.ContentType);

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