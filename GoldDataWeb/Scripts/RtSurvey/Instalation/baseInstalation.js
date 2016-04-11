var baseInstalation = (function () {
	return {
		init: function () {

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