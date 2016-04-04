var baseSite = (function () {
	return {
		init: function () {

		},

		SiteCreate: function (data) {
			return $.ajax({
				method: "POST",
				async: false,
				data: data,
				processData: false,
				contentType: "application/json",
				dataType: "json",
				url: base.GetRootStepOrderCreate(),
				error: base.ErrorAjax
			});
		}
	}
}());

$(function () {
	baseSite.init();
});