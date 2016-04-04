using System;
using System.Net;
using System.Web.Mvc;
using GD.Models.Commons;
using GD.Models.Commons.Utilities;
using GoldDataWeb.Controllers.Base;
using GoldDataWeb.Models;
using RestSharp;

namespace GoldDataWeb.Controllers
{
	public class StepsController : BaseController
	{
		[HttpPost]
		public JsonResult ClientCreate(StepsViewModel viewModel)
		{
			viewModel.Client.SiteNumber = @"0";
			viewModel.Client.CreateAt = DateTime.Now;
			viewModel.Client.CreateBy = short.Parse(GetAuthData().UserId.ToString());
			viewModel.Client.Status = new Status
			{
				Id = (int)Status.Type.Activo
			};
			var response = ClientService.Insert(viewModel.Client);
			return Json(response);
		}

		[HttpPost]
		public JsonResult ContactsCreate(StepsViewModel viewModel)
		{
			viewModel.EntityContact.CreateAt = DateTime.Now;
			viewModel.EntityContact.CreateBy = short.Parse(GetAuthData().UserId.ToString());
			viewModel.EntityContact.Status = new Status
			{
				Id = (int)Status.Type.Activo
			};
			var response = EntityContactService.Insert(viewModel.EntityContact);
			if ((string.IsNullOrWhiteSpace(response.ErrorMessage)) && (response.Status == HttpStatusCode.OK))
			{
				viewModel.EntityContact.Id = int.Parse(response.Data.ToString());

				foreach (var entityChannel in viewModel.EntityContact.ListEntityChannels)
				{
					entityChannel.Contact = new EntityContact
					{
						Id = viewModel.EntityContact.Id
					};
					entityChannel.CreateAt = DateTime.Now;
					entityChannel.CreateBy = short.Parse(GetAuthData().UserId.ToString());
					entityChannel.Status = new Status
					{
						Id = (int)Status.Type.Activo
					};
					var id = EntityChannelService.Insert(entityChannel);
				}
			}

			return Json(response);
		}

		[HttpPost]
		public JsonResult OrderCreate(StepsViewModel viewModel)
		{
			viewModel.Site.CreateAt = DateTime.Now;
			viewModel.Site.CreateBy = short.Parse(GetAuthData().UserId.ToString());
			viewModel.Site.Status = new Status
			{
				Id = (int)Status.Type.Activo
			};
			var responseSite = SiteService.Insert(viewModel.Site);
			if ((string.IsNullOrWhiteSpace(responseSite.ErrorMessage)) && (responseSite.Status == HttpStatusCode.OK))
			{
				viewModel.Site.Id = int.Parse(responseSite.Data.ToString());
				viewModel.Site.SiteNumber = SiteService.GetById(viewModel.Site.Id).Data.SiteNumber;
				foreach (var siteSchedule in viewModel.Site.ListSiteSchedule)
				{
					siteSchedule.Site = new Site
					{
						Id = viewModel.Site.Id
					};
					siteSchedule.CreateAt = DateTime.Now;
					siteSchedule.CreateBy = short.Parse(GetAuthData().UserId.ToString());
					siteSchedule.Status = new Status
					{
						Id = (int)Status.Type.Activo
					};
					var id = SiteScheduleService.Insert(siteSchedule);
				}

				var order = new Order
				{
					OrderNumber = viewModel.CountryAbbrevation + "-" + viewModel.Site.Client.Id.ToString().PadLeft(4, '0') + "-" + viewModel.Site.SiteNumber.ToString().PadLeft(2, '0'),
					OrderStatus = new OrderStatus
					{
						Id = (int)OrderStatus.Type.Nuevo
					},
					Site = new Site
					{
						Id = viewModel.Site.Id
					},
					CreateAt = DateTime.Now,
					CreateBy = short.Parse(GetAuthData().UserId.ToString()),
					Status = new Status
					{
						Id = (int)Status.Type.Activo
					}
				};

				var responseOrder = OrderService.Insert(order);
			}

			return Json(responseSite);
		}

		[HttpPost]
		public JsonResult PreFactibilityCreate(StepsViewModel viewModel)
		{
			var lineSight = new LineSight
			{
				RadioBase = new RadioBase
				{
					Id = viewModel.LineSight.RadioBase.Id
				},
				Site = new Site
				{
					Id = viewModel.LineSight.Site.Id
				},
				Distance = viewModel.LineSight.Distance,
				CreateAt = DateTime.Now,
				CreateBy = short.Parse(GetAuthData().UserId.ToString()),
				Status = new Status
				{
					Id = (int)Status.Type.Activo
				}
			};

			var responseLine = LineSightService.Insert(lineSight);

			var orderNew = new Order
			{
				Id = viewModel.Order.Id,
				OrderStatus = new OrderStatus
				{
					Id = viewModel.Order.OrderStatus.Id
				},
				Status = new Status
				{
					Id = viewModel.Order.Status.Id
				},
				CreateAt = DateTime.Now,
				CreateBy = short.Parse(GetAuthData().UserId.ToString())
			};

			var response = OrderService.Execute(@"updatestatus", Method.PUT, null, orderNew.ToJson());

			return Json(response);
		}

		[HttpPost]
		public JsonResult InspectionCreate(StepsViewModel viewModel)
		{
			var siteNew = new Site
			{
				Id = viewModel.Site.Id,
				BuildingFloors = viewModel.Site.BuildingFloors,
				BuildingHight = viewModel.Site.BuildingHight,
				FloorHight = viewModel.Site.FloorHight
			};

			var responseUpdateSite = SiteService.Execute(@"UpdateBuildingInformation", Method.PUT, null, siteNew.ToJson());

			var orderNew = new Order
			{
				Id = viewModel.Order.Id,
				SpecialRequirements = viewModel.Order.SpecialRequirements,
				Comments = viewModel.Order.Comments,
				AditionalCost = viewModel.Order.AditionalCost
			};

			var responseUpdateOrder = OrderService.Execute(@"UpdateInformation", Method.PUT, null, orderNew.ToJson());

			OrderService.Execute(@"DeleteBySite", Method.PUT, null, viewModel.Site.Id.ToString());

			//foreach (var siteAccessType in viewModel.Site.ListSiteAccessType)
			//{
			//	siteAccessType.Site = new Site
			//	{
			//		Id = viewModel.Site.Id
			//	};
			//	siteAccessType.AccessType = new AccessType
			//	{
			//		Id = siteAccessType.AccessType.Id
			//	};
			//	siteAccessType.CreateAt = DateTime.Now;
			//	siteAccessType.CreateBy = short.Parse(GetAuthData().UserId.ToString());
			//	siteAccessType.Status = new Status
			//	{
			//		Id = (int)Status.Type.Activo
			//	};
			//	var id = SiteAccessTypeService.Insert(siteAccessType);
			//}

			var updateStatus = new Order
			{
				Id = viewModel.Order.Id,
				OrderStatus = new OrderStatus
				{
					Id = viewModel.Order.OrderStatus.Id
				},
				Status = new Status
				{
					Id = viewModel.Order.Status.Id
				},
				CreateAt = DateTime.Now,
				CreateBy = short.Parse(GetAuthData().UserId.ToString())
			};

			var response = OrderService.Execute(@"updatestatus", Method.PUT, null, updateStatus.ToJson());
			response.Data = updateStatus;

			return Json(response);
		}

		public JsonResult InstalationCrete(StepsViewModel viewModel)
		{
			var orderNew = new Order
			{
				Id = viewModel.Order.Id,
				SettingUp = viewModel.Order.SettingUp
			};

			var responseUpdateOrder = OrderService.Execute(@"UpdateInformation", Method.PUT, null, orderNew.ToJson());

			foreach (var ordermaterial in viewModel.Order.ListMaterials)
			{
				ordermaterial.Order = new Order
				{
					Id = viewModel.Order.Id
				};
				ordermaterial.CreateAt = DateTime.Now;
				ordermaterial.CreateBy = short.Parse(GetAuthData().UserId.ToString());
				ordermaterial.Status = new Status
				{
					Id = (int)Status.Type.Activo
				};
				var id = OrderMaterialService.Insert(ordermaterial);
			}

			orderNew = new Order
			{
				Id = viewModel.Order.Id,
				OrderStatus = new OrderStatus
				{
					Id = viewModel.Order.OrderStatus.Id
				},
				Status = new Status
				{
					Id = viewModel.Order.Status.Id
				},
				CreateAt = DateTime.Now,
				CreateBy = short.Parse(GetAuthData().UserId.ToString())
			};

			var response = OrderService.Execute(@"updatestatus", Method.PUT, null, orderNew.ToJson());

			return Json(response);
		}
	}
}