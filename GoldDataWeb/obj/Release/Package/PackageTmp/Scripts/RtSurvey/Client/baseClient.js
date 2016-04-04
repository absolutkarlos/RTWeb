var baseClient = (function () {
	return {
		init: function () {

		},

		ClientCreate: function (data) {
			return $.ajax({
				method: "POST",
				async: false,
				data: data,
				dataType: "json",
				url: base.GetRootStepClientCreate(),
				error: base.ErrorAjax
			});
		}
	}
}());

$(function () {
	baseClient.init();
});