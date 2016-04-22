using System.Collections.Generic;
using GD.Models.Commons;

namespace GoldDataWeb.Models
{
	public class HomeViewModel
	{
		public IEnumerable<Order> Orders { get; set; }
		public User User { get; set; }
		public Order Order { get; set; }

		public StatusBarViewModel GetStatusBarViewModel()
		{
			var status = new StatusBarViewModel
			{
				Nuevo = new Data(),
				Prefactibility = new Data(),
				Instalation = new Data(),
				Inspection = new Data()
			};

			if (Order != null)
			{
				foreach (var orderFlow in Order.ListOrderFlow)
				{
					if (orderFlow != null)
					{
						switch (orderFlow.IdOrderStatus)
						{
							case (int) OrderStatus.Type.Nuevo:
								status.Nuevo = new Data
								{
									IdOrderFlow = orderFlow.Id.ToString(),
									EstimatedDateNextStep = orderFlow.StimatedDateNextStep?.ToLongDateString(),
									EffectiveDate = orderFlow.UpdateAt?.ToLongDateString() ?? orderFlow.CreateAt?.ToLongDateString(),
									Class = (orderFlow.Status.Id == (int) Status.Type.Activo)
											? @"glyphicon-ok"
											: (orderFlow.Status.Id == (int) Status.Type.Inactivo) ? @"glyphicon-remove" : string.Empty,
									Color = (orderFlow.Status.Id == (int) Status.Type.Activo) ? @"limegreen" : @"crimson"
								};
								break;
							case (int) OrderStatus.Type.Prefactibilidad:
								status.Prefactibility = new Data
								{
									IdOrderFlow = orderFlow.Id.ToString(),
									EstimatedDateNextStep = (orderFlow.Status.Id == (int) Status.Type.Activo && orderFlow.StimatedDateNextStep != null)
											? orderFlow.StimatedDateNextStep?.ToLongDateString()
											: @"NO DISPONIBLE",
									EffectiveDate = orderFlow.UpdateAt?.ToLongDateString() ?? orderFlow.CreateAt?.ToLongDateString(),
									Class = (orderFlow.Status.Id == (int) Status.Type.Activo)
											? @"glyphicon-ok"
											: (orderFlow.Status.Id == (int) Status.Type.Inactivo) ? @"glyphicon-remove" : string.Empty,
									Color = (orderFlow.Status.Id == (int) Status.Type.Activo) ? @"limegreen" : @"crimson"
								};
								break;
							case (int) OrderStatus.Type.Inspeccion:
								status.Inspection = new Data
								{
									IdOrderFlow = orderFlow.Id.ToString(),
									EstimatedDateNextStep = (orderFlow.Status.Id == (int)Status.Type.Activo && orderFlow.StimatedDateNextStep != null)
											? orderFlow.StimatedDateNextStep?.ToLongDateString()
											: @"NO DISPONIBLE",
									EffectiveDate = orderFlow.UpdateAt?.ToLongDateString() ?? orderFlow.CreateAt?.ToLongDateString(),
									Class = (orderFlow.Status.Id == (int) Status.Type.Activo)
											? @"glyphicon-ok"
											: (orderFlow.Status.Id == (int) Status.Type.Inactivo) ? @"glyphicon-remove" : string.Empty,
									Color = (orderFlow.Status.Id == (int) Status.Type.Activo) ? @"limegreen" : @"crimson"
								};
								break;
							case (int) OrderStatus.Type.Instalacion:
								status.Instalation = new Data
								{
									IdOrderFlow = orderFlow.Id.ToString(),
									EstimatedDateNextStep = (orderFlow.Status.Id == (int)Status.Type.Activo && orderFlow.StimatedDateNextStep != null)
											? orderFlow.StimatedDateNextStep?.ToLongDateString()
											: @"NO DISPONIBLE",
									EffectiveDate = orderFlow.UpdateAt?.ToLongDateString() ?? orderFlow.CreateAt?.ToLongDateString(),
									Class = (orderFlow.Status.Id == (int) Status.Type.Activo)
											? @"glyphicon-ok"
											: (orderFlow.Status.Id == (int) Status.Type.Inactivo) ? @"glyphicon-remove" : string.Empty,
									Color = (orderFlow.Status.Id == (int) Status.Type.Activo) ? @"limegreen" : @"crimson"
								};
								break;
						}
					}
				}
			}
			return status;
		}
	}
}