using System.Collections.Generic;
using System.Web.Mvc;
using GD.Models.Commons;
using GoldDataWeb.Controllers.Base;
using RestSharp;

namespace GoldDataWeb.Controllers
{
	public class MetaDataController : BaseController
	{
		// GET: MetaData
		public JsonResult Index()
		{
			var metadata = new
			{
				Roles = RolService.GetAll(),
				Countries = CountryService.GetAll(),
				AccessType = AccessTypeService.GetAll(),
				Materials = MaterialService.GetAll(),
				ServiceType = ServiceTypeService.GetAll(),
				EntityType = EntityTypeService.GetAll(),
				EntityChannelType = EntityChannelTypeService.GetAll(),
				Position = PositionService.GetAll(),
				OrderStatus = OrderStatusService.GetAll(),
				OrderShotType = OrderShotTypeService.GetAll(),
				RadioBase = RadioBaseService.GetAll(),
				ClientType = ClientTypeService.GetAll()
			};
			return Json(metadata, JsonRequestBehavior.AllowGet);
		}

		// GET: MetaData
		public JsonResult Clients()
		{
			var metadata = ClientService.GetAll();

			return Json(metadata, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetClient(int clientId)
		{
			var metadata = ClientService.Execute<Client>(@"getinfo", Method.GET, clientId.ToString());

			return Json(metadata, JsonRequestBehavior.AllowGet);
		}

		// GET: MetaData
		public JsonResult State(int countryId)
		{
			var metadata = StateService.Execute<IEnumerable<State>>(@"getbycountry", Method.GET, countryId.ToString());

			return Json(metadata, JsonRequestBehavior.AllowGet);
		}

		// GET: MetaData
		public JsonResult City(int stateId)
		{
			var metadata =  CityService.Execute<IEnumerable<City>>(@"getbystate", Method.GET, stateId.ToString());

			return Json(metadata, JsonRequestBehavior.AllowGet);
		}

		// GET: MetaData
		public JsonResult Zone(int cityId)
		{
			var metadata = ZoneService.Execute<IEnumerable<Zone>>(@"getbycity", Method.GET, cityId.ToString());

			return Json(metadata, JsonRequestBehavior.AllowGet);
		}
	}
}