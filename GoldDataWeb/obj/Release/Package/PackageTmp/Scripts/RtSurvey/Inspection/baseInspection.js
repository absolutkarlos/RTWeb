var baseInspection = (function () {
	return {
		init: function () {

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