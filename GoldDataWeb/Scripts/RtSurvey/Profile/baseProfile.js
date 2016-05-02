var baseProfile = (function () {
	return {
		init: function () {

		},

		UpdateProfile: function (data) {
			return $.ajax({
				method: "POST",
				async: false,
				data: data,
				contentType: "application/json",
				dataType: "json",
				url: base.GetRootUpdateProfile(),
				error: base.ErrorAjax
			});
		}
	}
}());

$(function () {
	baseProfile.init();
});