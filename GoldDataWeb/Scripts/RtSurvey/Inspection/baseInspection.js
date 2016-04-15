var baseInspection = (function () {
	return {
		init: function () {

		},

		LoadInspectionPanel: function (orderId) {
			return $.ajax({
				method: "GET",
				dataType: "html",
				data: {
					"orderId": orderId
				},
				url: base.GetRootInspectionPanel(),
				error: base.ErrorAjax
			});
		},

		InspectionCreate: function (data) {
			return $.ajax({
				method: "POST",
				async: false,
				data: data,
				processData: false,
				contentType: "application/json",
				dataType: "json",
				url: base.GetRootStepInspectionCreate(),
				error: base.ErrorAjax
			});
		}
	}
}());

$(function () {
	baseInspection.init();
});