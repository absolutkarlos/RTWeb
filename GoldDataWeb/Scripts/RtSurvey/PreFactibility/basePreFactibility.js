var basePreFactibility = (function () {
	return {
		init: function () {

		},

		LineSightCreate: function (data) {
			return $.ajax({
				method: "POST",
				async: false,
				dataType: "json",
				data: data,
				processData: false,
				contentType: "application/json",
				url: base.GetRootStepPreFactibilityCreate(),
				error: base.ErrorAjax
			});
		}
	}
}());

$(function () {
	basePreFactibility.init();
});