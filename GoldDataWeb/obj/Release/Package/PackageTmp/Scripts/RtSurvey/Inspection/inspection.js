var inspection = (function () {
	return {
		init: function () {

		},

		InspectionCreate: function (data) {
			$.when(baseInspection.InspectionCreate(data)).then(function (response) {
				home.UpdateOrderPanel();
				home.LoadInfoOrderPanel($("#orderIdLabel").data("orderid"));
				if ((response.Data) && (response.Data.Status) && (response.Data.Status.Id === 1)) {
					//$('#wizardInfo').bootstrapWizard('show', 2);
				}
			});
		},

		GetSiteAccessTypeList: function () {
			var accessTypes = [];
			if ($("#accesstype").val().length > 0) {
				$.each($("#accesstype").val(), function (index, item) {
					accessTypes.push({
						"AccessType.Id": item
					});
				});
			}
			return accessTypes;
		},

		GetInspection: function (statusId) {
			var inspectionViewModel = {
				"Order.Id": $("#orderIdLabel").data("orderid"),
				"Order.Comments": $("#coment").val(),
				"Order.AditionalCost": $("#costo").val(),
				"Order.SpecialRequirements": $("#requerimientos").val(),
				"Order.OrderStatus.Id": parseInt($("#orderIdLabel").data("orderstatustype")) + 1,
				"Order.Status.Id": statusId,
				"Site.Id": $("#orderIdLabel").data("siteid"),
				"Site.FloorHight": $("#alturapiso").val(),
				"Site.BuildingHight": $("#altura").val(),
				"Site.BuildingFloors": $("#cantidadpisos").val(),
				"Site.ListSiteAccessType": this.GetSiteAccessTypeList()
			}
			return JSON.stringify(inspectionViewModel);
		},

		GetEvent: function () {
			return {
				GenerateInspectionEvent: function () {
					$("#rechazarInspection, #aprobarInspection").click(function () {
						var statusId = $(this).data("status");
						$("#titleModalAction").text($(this).val());
						if ($("#wizardInfoForm").valid()) {
							$('#confirm').modal({ backdrop: 'static', keyboard: false }).one('click', '#buttomModalConfirm', function () {
								inspection.InspectionCreate(inspection.GetInspection(statusId));
							});
						}
					});
				}

			}
		}
	}
}());

$(function () {
	inspection.init();
});