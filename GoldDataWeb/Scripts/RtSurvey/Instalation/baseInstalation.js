var baseInstalation = (function () {
	return {
		init: function () {

		},

		LoadInstalationPanel: function (orderId) {
			return $.ajax({
				method: "GET",
				dataType: "html",
				data: {
					"orderId": orderId
				},
				url: base.GetRootInstalationPanel(),
				error: base.ErrorAjax
			});
		},

		InstalationCreate: function (data) {
			return $.ajax({
				method: "POST",
				async: false,
				data: data,
				processData: false,
				contentType: "application/json",
				dataType: "json",
				url: base.GetRootStepInstalationCreate(),
				error: base.ErrorAjax
			});
		}
	}
}());

$(function () {
	baseInstalation.init();
});